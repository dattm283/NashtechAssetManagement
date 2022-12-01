using AssetManagement.Application.Controllers;
using AssetManagement.Contracts.AutoMapper;
using AssetManagement.Contracts.Common;
using AssetManagement.Contracts.User.Request;
using AssetManagement.Contracts.User.Response;
using AssetManagement.Data.EF;
using AssetManagement.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Principal;
using Xunit;

namespace AssetManagement.Application.Tests
{
    public class UsersControllerTests : IDisposable
    {
        private readonly DbContextOptions _options;
        private readonly AssetManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly Mock<UserManager<AppUser>> _userManager;
        private List<AppRole> _roles;
        private List<AppUser> _users;

        public UsersControllerTests()
        {
            //Mock UserManager
            //Create UserStore mock to enable user support for UserManager
            Mock<IUserStore<AppUser>> userStoreMoq = new();
            //Create UserManager mock using userStoreMoq
            _userManager = new(userStoreMoq.Object, null, null, null, null, null, null, null, null);
            // Create InMemory dbcontext options
            _options = new DbContextOptionsBuilder<AssetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "AssetTestDb").Options;

            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new UserProfile())).CreateMapper();

            // Create InMemory dbcontext with options
            _context = new AssetManagementDbContext(_options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        #region Change User Password

        #region User Change Password Success
        [Theory]
        [InlineData("12345678", "123456")]
        public async Task UserChangePassword_SuccessAsync(string currentpassword, string newpassword)
        {
            //ARRANGE
            UserChangePasswordRequest request = new() { CurrentPassword = currentpassword, NewPassword = newpassword };
            UserController userController = new UserController(_context, _userManager.Object, _mapper);
            List<AppUser> listUsers = _context.AppUsers.ToList();
            AppUser currentUser = listUsers.ElementAt(0);

            //Set up UserManager
            _userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(currentUser);
            _userManager.Setup(um => um.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(true));
            _userManager.Setup(um => um.ChangePasswordAsync(currentUser, currentpassword, newpassword))
                        .ReturnsAsync(IdentityResult.Success);

            UserController controller = new UserController(_context, _userManager.Object, _mapper);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(currentUser.UserName), null)
                }
            };

            //ACT
            IActionResult result = controller.ChangePassword(request).Result;
            string message = ((SuccessResponseResult<string>)((ObjectResult)result).Value).Result;

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Change password success!", message);
        }
        #endregion

        #region Wrong Password Request
        [Theory]
        [InlineData("123456", "123456")]
        public async Task UserChangePassword_WrongPassword_ReturnBadRequest(string currentpassword, string newpassword)
        {
            UserChangePasswordRequest request = new() { CurrentPassword = currentpassword, NewPassword = newpassword };
            UserController userController = new UserController(_context, _userManager.Object, _mapper);
            List<AppUser> listUsers = _context.AppUsers.ToList();
            AppUser currentUser = listUsers.ElementAt(0);

            //Set up UserManager
            _userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(currentUser);
            _userManager.Setup(um => um.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(false));

            UserController controller = new UserController(_context, _userManager.Object, _mapper);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(currentUser.UserName), null)
                }
            };

            //ACT
            IActionResult result = controller.ChangePassword(request).Result;
            string message = ((ErrorResponseResult<string>)((ObjectResult)result).Value).Message;

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Password doesn't match", message);
        }
        #endregion

        #region NewPassword Is Old Password
        [Theory]
        [InlineData("12345678", "12345678")]
        public async Task UserChangePassword_NewPasswordIsOldPassword_ReturnBadRequest(string currentpassword, string newpassword)
        {
            UserChangePasswordRequest request = new() { CurrentPassword = currentpassword, NewPassword = newpassword };
            UserController userController = new UserController(_context, _userManager.Object, _mapper);
            List<AppUser> listUsers = _context.AppUsers.ToList();
            AppUser currentUser = listUsers.ElementAt(0);

            //Set up UserManager
            _userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(currentUser);
            _userManager.Setup(um => um.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(true));

            UserController controller = new UserController(_context, _userManager.Object, _mapper);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(currentUser.UserName), null)
                }
            };

            //ACT
            IActionResult result = controller.ChangePassword(request).Result;
            string message = ((ErrorResponseResult<string>)((ObjectResult)result).Value).Message;

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("New password must be different", message);
        }
        #endregion

        #region Model State Invalid
        [Theory]
        [InlineData("12345", null)]
        [InlineData("12345678", null)]
        [InlineData(null, null)]
        [InlineData(null, "12345")]
        [InlineData(null, "1234567")]
        public async Task UserChangePassword_BadRequest_ModelState_Invalid(string currentpassword, string newpassword)
        {
            UserChangePasswordRequest request = new() { CurrentPassword = currentpassword, NewPassword = newpassword };
            UserController userController = new UserController(_context, _userManager.Object, _mapper);
            List<AppUser> listUsers = _context.AppUsers.ToList();
            AppUser currentUser = listUsers.ElementAt(0);

            //Set up UserManager
            _userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(currentUser);
            //_userManager.Setup(um => um.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            //        .Returns(Task.FromResult(false));

            //Create controller
            UserController controller = new UserController(_context, _userManager.Object, _mapper);
            if (currentpassword == null) controller.ModelState.AddModelError("nullcurrentpassword", "Please enter old password");
            if (newpassword == null) controller.ModelState.AddModelError("nullnewpassword", "Please enter new password");
            if (currentpassword != null && currentpassword.Length < 6) controller.ModelState.AddModelError("invalidcurrentpassword", "Please enter valid password");
            if (newpassword != null && newpassword.Length < 6) controller.ModelState.AddModelError("invalidnewpassword", "Please enter valid password");

            //ACT
            IActionResult result = controller.ChangePassword(request).Result;
            SerializableError errors = (SerializableError)((ObjectResult)result).Value;

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            if (errors != null && errors.ContainsKey("nullcurrentpassword"))
            {
                string message = (errors["nullcurrentpassword"] as string[]).FirstOrDefault();
                Assert.Equal("Please enter old password", message);
            }
            if (errors != null && errors.ContainsKey("nullnewpassword"))
            {
                string message = (errors["nullnewpassword"] as string[]).FirstOrDefault();
                Assert.Equal("Please enter new password", message);
            }
            if (errors != null && errors.ContainsKey("invalidcurrentpassword"))
            {
                string message = (errors["invalidcurrentpassword"] as string[]).FirstOrDefault();
                Assert.Equal("Please enter valid password", message);
            }
            if (errors != null && errors.ContainsKey("invalidnewpassword"))
            {
                string message = (errors["invalidnewpassword"] as string[]).FirstOrDefault();
                Assert.Equal("Please enter valid password", message);
            }
        }
        #endregion

        #endregion

        #region GetUser
        [Fact]
        public async Task GetList_ForDefault_Ok()
        {
            // Arrange 
            #region Arrange
            UserController userController = new UserController(_context, _userManager.Object, _mapper);
            List<AppUser> listUsers = _context.AppUsers.ToList();
            AppUser currentUser = listUsers.ElementAt(0);
            //Create context for controller with fake login
            userController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(currentUser.UserName), null)
                }
            };

            List<AppUser> addminRole = await _context.AppUsers
               .Where(x => !x.IsDeleted && x.UserName.Contains("admin"))
               .ToListAsync();
            List<AppUser> staffRole = await _context.AppUsers
               .Where(x => !x.IsDeleted && x.UserName.Contains("staff"))
               .ToListAsync();
            foreach(AppUser user in listUsers)
            {
                if (addminRole.Contains(user))
                    _userManager.Setup(_ => _.GetRolesAsync(user).Result).Returns(new List<string> { "Admin"});
                else
                    _userManager.Setup(_ => _.GetRolesAsync(user).Result).Returns(new List<string> { "Staff" });
            }
            _userManager.Setup(_ => _.GetUsersInRoleAsync("Admin").Result).Returns(addminRole);
            _userManager.Setup(_ => _.GetUsersInRoleAsync("Staff").Result).Returns(staffRole);
            #endregion

            // Act 
            #region Act
            List<AppUser> expectedResult = await _context.AppUsers
                .Where(
                    x => !x.IsDeleted && 
                    x.Location == currentUser.Location &&
                    (addminRole.Contains(x) || staffRole.Contains(x)))
                .OrderBy(x => x.StaffCode)
                .ToListAsync();

            var result = await userController.GetAllUser(0, 2, "", "", "staffCode", "ASC");
            var okobjectResult = result.Result as OkObjectResult;
            ViewList_ListResponse<ViewListUser_UserResponse> actualResult = 
                okobjectResult.Value as ViewList_ListResponse<ViewListUser_UserResponse>;
            #endregion

            // Assert
            #region Assert
            Assert.NotNull(actualResult);
            Assert.Equal(actualResult.Total, expectedResult.Count);
            for(int i=0; i<expectedResult.Count; i++)
            {
                Assert.Equal(actualResult.Data.ElementAt(i).UserName, expectedResult.ElementAt(i).UserName);
            }
            #endregion
        }

        [Fact]
        public async Task GetList_SearchString_Ok()
        {
            // Arrange 
            #region Arrange
            string searchString = "SD";
            UserController userController = new UserController(_context, _userManager.Object, _mapper);
            List<AppUser> listUsers = _context.AppUsers.ToList();
            AppUser currentUser = listUsers.ElementAt(0);
            //Create context for controller with fake login
            userController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(currentUser.UserName), null)
                }
            };

            List<AppUser> addminRole = await _context.AppUsers
               .Where(x => !x.IsDeleted && x.UserName.Contains("admin"))
               .ToListAsync();
            List<AppUser> staffRole = await _context.AppUsers
               .Where(x => !x.IsDeleted && x.UserName.Contains("staff"))
               .ToListAsync();
            foreach (AppUser user in listUsers)
            {
                if (addminRole.Contains(user))
                    _userManager.Setup(_ => _.GetRolesAsync(user).Result).Returns(new List<string> { "Admin" });
                else
                    _userManager.Setup(_ => _.GetRolesAsync(user).Result).Returns(new List<string> { "Staff" });
            }
            _userManager.Setup(_ => _.GetUsersInRoleAsync("Admin").Result).Returns(addminRole);
            _userManager.Setup(_ => _.GetUsersInRoleAsync("Staff").Result).Returns(staffRole);
            #endregion

            // Act 
            #region Act
            List<AppUser> expectedResult = await _context.AppUsers
                .Where(x => 
                    !x.IsDeleted && 
                    x.Location == currentUser.Location && 
                    (x.StaffCode.Contains(searchString) || $"{x.FirstName} {x.LastName}".Contains(searchString)) &&
                    (addminRole.Contains(x) || staffRole.Contains(x)))
                .OrderBy(x => x.StaffCode)
                .ToListAsync();

            var result = await userController.GetAllUser(0, 2, "", searchString, "staffCode", "ASC");
            var okobjectResult = result.Result as OkObjectResult;
            ViewList_ListResponse<ViewListUser_UserResponse> actualResult =
                okobjectResult.Value as ViewList_ListResponse<ViewListUser_UserResponse>;
            #endregion

            // Assert
            #region Assert
            Assert.NotNull(actualResult);
            Assert.Equal(actualResult.Total, expectedResult.Count);
            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.Equal(actualResult.Data.ElementAt(i).UserName, expectedResult.ElementAt(i).UserName);
            }
            #endregion
        }

        [Fact]
        public async Task GetList_FilterState_Ok()
        {
            // Arrange 
            #region Arrange
            string filterState = "Admin&Staff&";
            var listType = filterState.Split("&").ToArray();

            UserController userController = new UserController(_context, _userManager.Object, _mapper);
            List<AppUser> listUsers = _context.AppUsers.ToList();
            AppUser currentUser = listUsers.ElementAt(0);
            //Create context for controller with fake login
            userController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(currentUser.UserName), null)
                }
            };

            List<AppUser> addminRole = await _context.AppUsers
               .Where(x => !x.IsDeleted && x.UserName.Contains("admin"))
               .ToListAsync();
            List<AppUser> staffRole = await _context.AppUsers
               .Where(x => !x.IsDeleted && x.UserName.Contains("staff"))
               .ToListAsync();
            foreach (AppUser user in listUsers)
            {
                if (addminRole.Contains(user))
                    _userManager.Setup(_ => _.GetRolesAsync(user).Result).Returns(new List<string> { "Admin" });
                else
                    _userManager.Setup(_ => _.GetRolesAsync(user).Result).Returns(new List<string> { "Staff" });
            }
            _userManager.Setup(_ => _.GetUsersInRoleAsync("Admin").Result).Returns(addminRole);
            _userManager.Setup(_ => _.GetUsersInRoleAsync("Staff").Result).Returns(staffRole);
            #endregion

            // Act 
            #region Act
            List<AppUser> tempData = new List<AppUser>();
            for (int i = 0; i < listType.Length - 1; i++)
            {
                List<AppUser> tempUser = (listType[i] == "Admin") ? addminRole : staffRole;
                tempData.AddRange(tempUser);
            }
            List<AppUser> expectedResult = await _context.AppUsers
                .Where(x =>
                    !x.IsDeleted &&
                    x.Location == currentUser.Location &&
                    tempData.Contains(x))
                .OrderBy(x => x.StaffCode)
                .ToListAsync();

            var result = await userController.GetAllUser(0, 2, filterState, "", "staffCode", "ASC");
            var okobjectResult = result.Result as OkObjectResult;
            ViewList_ListResponse<ViewListUser_UserResponse> actualResult =
                okobjectResult.Value as ViewList_ListResponse<ViewListUser_UserResponse>;
            #endregion

            // Assert
            #region Assert
            Assert.NotNull(actualResult);
            Assert.Equal(actualResult.Total, expectedResult.Count);
            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.Equal(actualResult.Data.ElementAt(i).UserName, expectedResult.ElementAt(i).UserName);
            }
            #endregion
        }

        [Fact]
        public async Task GetList_SortByUserName_Ok()
        {
            // Arrange 
            #region Arrange
            string sort = "userName";

            UserController userController = new UserController(_context, _userManager.Object, _mapper);
            List<AppUser> listUsers = _context.AppUsers.ToList();
            AppUser currentUser = listUsers.ElementAt(0);
            //Create context for controller with fake login
            userController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(currentUser.UserName), null)
                }
            };

            List<AppUser> addminRole = await _context.AppUsers
               .Where(x => !x.IsDeleted && x.UserName.Contains("admin"))
               .ToListAsync();
            List<AppUser> staffRole = await _context.AppUsers
               .Where(x => !x.IsDeleted && x.UserName.Contains("staff"))
               .ToListAsync();
            foreach (AppUser user in listUsers)
            {
                if (addminRole.Contains(user))
                    _userManager.Setup(_ => _.GetRolesAsync(user).Result).Returns(new List<string> { "Admin" });
                else
                    _userManager.Setup(_ => _.GetRolesAsync(user).Result).Returns(new List<string> { "Staff" });
            }
            _userManager.Setup(_ => _.GetUsersInRoleAsync("Admin").Result).Returns(addminRole);
            _userManager.Setup(_ => _.GetUsersInRoleAsync("Staff").Result).Returns(staffRole);
            #endregion

            // Act 
            #region Act
            List<AppUser> expectedResult = await _context.AppUsers
                .Where(x =>
                    !x.IsDeleted &&
                    x.Location == currentUser.Location)
                .OrderBy(x => x.UserName)
                .ToListAsync();

            var result = await userController.GetAllUser(0, 2, "", "", sort, "ASC");
            var okobjectResult = result.Result as OkObjectResult;
            ViewList_ListResponse<ViewListUser_UserResponse> actualResult =
                okobjectResult.Value as ViewList_ListResponse<ViewListUser_UserResponse>;
            #endregion

            // Assert
            #region Assert
            Assert.NotNull(actualResult);
            Assert.Equal(actualResult.Total, expectedResult.Count);
            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.Equal(actualResult.Data.ElementAt(i).UserName, expectedResult.ElementAt(i).UserName);
            }
            #endregion
        }

        [Fact]
        public async Task GetList_SortByFullName_Ok()
        {
            // Arrange 
            #region Arrange
            string sort = "fullName";

            UserController userController = new UserController(_context, _userManager.Object, _mapper);
            List<AppUser> listUsers = _context.AppUsers.ToList();
            AppUser currentUser = listUsers.ElementAt(0);
            //Create context for controller with fake login
            userController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(currentUser.UserName), null)
                }
            };

            List<AppUser> addminRole = await _context.AppUsers
               .Where(x => !x.IsDeleted && x.UserName.Contains("admin"))
               .ToListAsync();
            List<AppUser> staffRole = await _context.AppUsers
               .Where(x => !x.IsDeleted && x.UserName.Contains("staff"))
               .ToListAsync();
            foreach (AppUser user in listUsers)
            {
                if (addminRole.Contains(user))
                    _userManager.Setup(_ => _.GetRolesAsync(user).Result).Returns(new List<string> { "Admin" });
                else
                    _userManager.Setup(_ => _.GetRolesAsync(user).Result).Returns(new List<string> { "Staff" });
            }
            _userManager.Setup(_ => _.GetUsersInRoleAsync("Admin").Result).Returns(addminRole);
            _userManager.Setup(_ => _.GetUsersInRoleAsync("Staff").Result).Returns(staffRole);
            #endregion

            // Act 
            #region Act
            List<AppUser> expectedResult = await _context.AppUsers
                .Where(x =>
                    !x.IsDeleted &&
                    x.Location == currentUser.Location)
                .OrderBy(x => x.FirstName + ' ' + x.LastName)
                .ToListAsync();

            var result = await userController.GetAllUser(0, 2, "", "", sort, "ASC");
            var okobjectResult = result.Result as OkObjectResult;
            ViewList_ListResponse<ViewListUser_UserResponse> actualResult =
                okobjectResult.Value as ViewList_ListResponse<ViewListUser_UserResponse>;
            #endregion

            // Assert
            #region Assert
            Assert.NotNull(actualResult);
            Assert.Equal(actualResult.Total, expectedResult.Count);
            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.Equal(actualResult.Data.ElementAt(i).UserName, expectedResult.ElementAt(i).UserName);
            }
            #endregion
        }

        [Fact]
        public async Task GetList_SortByJoinedDate_Ok()
        {
            // Arrange 
            #region Arrange
            string sort = "joinedDate";

            UserController userController = new UserController(_context, _userManager.Object, _mapper);
            List<AppUser> listUsers = _context.AppUsers.ToList();
            AppUser currentUser = listUsers.ElementAt(0);
            //Create context for controller with fake login
            userController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(currentUser.UserName), null)
                }
            };

            List<AppUser> addminRole = await _context.AppUsers
               .Where(x => !x.IsDeleted && x.UserName.Contains("admin"))
               .ToListAsync();
            List<AppUser> staffRole = await _context.AppUsers
               .Where(x => !x.IsDeleted && x.UserName.Contains("staff"))
               .ToListAsync();
            foreach (AppUser user in listUsers)
            {
                if (addminRole.Contains(user))
                    _userManager.Setup(_ => _.GetRolesAsync(user).Result).Returns(new List<string> { "Admin" });
                else
                    _userManager.Setup(_ => _.GetRolesAsync(user).Result).Returns(new List<string> { "Staff" });
            }
            _userManager.Setup(_ => _.GetUsersInRoleAsync("Admin").Result).Returns(addminRole);
            _userManager.Setup(_ => _.GetUsersInRoleAsync("Staff").Result).Returns(staffRole);
            #endregion

            // Act 
            #region Act
            List<AppUser> expectedResult = await _context.AppUsers
                .Where(x =>
                    !x.IsDeleted &&
                    x.Location == currentUser.Location)
                .OrderBy(x => x.CreatedDate)
                .ToListAsync();

            var result = await userController.GetAllUser(0, 2, "", "", sort, "ASC");
            var okobjectResult = result.Result as OkObjectResult;
            ViewList_ListResponse<ViewListUser_UserResponse> actualResult =
                okobjectResult.Value as ViewList_ListResponse<ViewListUser_UserResponse>;
            #endregion

            // Assert
            #region Assert
            Assert.NotNull(actualResult);
            Assert.Equal(actualResult.Total, expectedResult.Count);
            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.Equal(actualResult.Data.ElementAt(i).UserName, expectedResult.ElementAt(i).UserName);
            }
            #endregion
        }

        [Fact]
        public async Task GetList_SortByType_Ok()
        {
            // Arrange 
            #region Arrange
            string sort = "type";

            UserController userController = new UserController(_context, _userManager.Object, _mapper);
            List<AppUser> listUsers = _context.AppUsers.ToList();
            AppUser currentUser = listUsers.ElementAt(0);
            //Create context for controller with fake login
            userController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(currentUser.UserName), null)
                }
            };

            List<AppUser> addminRole = await _context.AppUsers
               .Where(x => !x.IsDeleted && x.UserName.Contains("admin"))
               .ToListAsync();
            List<AppUser> staffRole = await _context.AppUsers
               .Where(x => !x.IsDeleted && x.UserName.Contains("staff"))
               .ToListAsync();
            foreach (AppUser user in listUsers)
            {
                if (addminRole.Contains(user))
                    _userManager.Setup(_ => _.GetRolesAsync(user).Result).Returns(new List<string> { "Admin" });
                else
                    _userManager.Setup(_ => _.GetRolesAsync(user).Result).Returns(new List<string> { "Staff" });
            }
            _userManager.Setup(_ => _.GetUsersInRoleAsync("Admin").Result).Returns(addminRole);
            _userManager.Setup(_ => _.GetUsersInRoleAsync("Staff").Result).Returns(staffRole);
            #endregion

            // Act 
            #region Act
            IQueryable<AppUser> adminAccount = _context.AppUsers
                .Where(x =>
                    !x.IsDeleted &&
                    x.Location == currentUser.Location &&
                    addminRole.Contains(x));
            IQueryable<AppUser> staffAccount = _context.AppUsers
                .Where(x =>
                    !x.IsDeleted &&
                    x.Location == currentUser.Location &&
                    staffRole.Contains(x));
            List<AppUser> expectedResult = await adminAccount.Concat(staffAccount).ToListAsync();

            var result = await userController.GetAllUser(0, 2, "", "", sort, "ASC");
            var okobjectResult = result.Result as OkObjectResult;
            ViewList_ListResponse<ViewListUser_UserResponse> actualResult =
                okobjectResult.Value as ViewList_ListResponse<ViewListUser_UserResponse>;
            #endregion

            // Assert
            #region Assert
            Assert.NotNull(actualResult);
            Assert.Equal(actualResult.Total, expectedResult.Count);
            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.Equal(actualResult.Data.ElementAt(i).UserName, expectedResult.ElementAt(i).UserName);
            }
            #endregion
        }

        [Fact]
        public async Task GetList_InvalidPaging_Ok()
        {
            // Arrange 
            #region Arrange
            int start = -1;
            int end = 20;

            UserController userController = new UserController(_context, _userManager.Object, _mapper);
            List<AppUser> listUsers = _context.AppUsers.ToList();
            AppUser currentUser = listUsers.ElementAt(0);
            //Create context for controller with fake login
            userController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(currentUser.UserName), null)
                }
            };

            List<AppUser> addminRole = await _context.AppUsers
               .Where(x => !x.IsDeleted && x.UserName.Contains("admin"))
               .ToListAsync();
            List<AppUser> staffRole = await _context.AppUsers
               .Where(x => !x.IsDeleted && x.UserName.Contains("staff"))
               .ToListAsync();
            foreach (AppUser user in listUsers)
            {
                if (addminRole.Contains(user))
                    _userManager.Setup(_ => _.GetRolesAsync(user).Result).Returns(new List<string> { "Admin" });
                else
                    _userManager.Setup(_ => _.GetRolesAsync(user).Result).Returns(new List<string> { "Staff" });
            }
            _userManager.Setup(_ => _.GetUsersInRoleAsync("Admin").Result).Returns(addminRole);
            _userManager.Setup(_ => _.GetUsersInRoleAsync("Staff").Result).Returns(staffRole);
            #endregion

            // Act 
            #region Act
            IQueryable<AppUser> sortedListUser = _context.AppUsers
                .Where(x =>
                    !x.IsDeleted &&
                    x.Location == currentUser.Location)
                .OrderBy(x => x.UserName)
                .AsQueryable();
            List<AppUser> expectedResult = await sortedListUser
                .Skip(start < 0 || start > end ? 1 : start)
                .Take(end>sortedListUser.Count() ? sortedListUser.Count()-start : end-start)
                .ToListAsync();


            var result = await userController.GetAllUser(start, end, "", "", "staffCode", "ASC");
            var okobjectResult = result.Result as OkObjectResult;
            ViewList_ListResponse<ViewListUser_UserResponse> actualResult =
                okobjectResult.Value as ViewList_ListResponse<ViewListUser_UserResponse>;
            #endregion

            // Assert
            #region Assert
            Assert.NotNull(actualResult);
            Assert.Equal(actualResult.Total, expectedResult.Count);
            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.Equal(actualResult.Data.ElementAt(i).UserName, expectedResult.ElementAt(i).UserName);
            }
            #endregion
        }
        #endregion

        #region GetSingleUser
        [Fact]
        public async Task GetSingleUser_Ok()
        {
            // Arrange 
            #region Arrange
            AppUser expectedUser = _context.AppUsers.ToList().ElementAt(1);
            string staffCode = expectedUser.StaffCode;

            UserController userController = new UserController(_context, _userManager.Object, _mapper);
            List<AppUser> listUsers = _context.AppUsers.ToList();
            AppUser currentUser = listUsers.ElementAt(0);
            List<AppUser> addminRole = await _context.AppUsers
               .Where(x => !x.IsDeleted && x.UserName.Contains("admin"))
               .ToListAsync();
            List<AppUser> staffRole = await _context.AppUsers
               .Where(x => !x.IsDeleted && x.UserName.Contains("staff"))
               .ToListAsync();
            foreach (AppUser user in listUsers)
            {
                if (addminRole.Contains(user))
                    _userManager.Setup(_ => _.GetRolesAsync(user).Result).Returns(new List<string> { "Admin" });
                else
                    _userManager.Setup(_ => _.GetRolesAsync(user).Result).Returns(new List<string> { "Staff" });
            }
            #endregion

            // Act 
            #region Act
            var result = await userController.GetSingleUser(staffCode);
            var okobjectResult = result.Result as OkObjectResult;
            SuccessResponseResult<ViewDetailUser_UserResponse> actualResult =
                okobjectResult.Value as SuccessResponseResult<ViewDetailUser_UserResponse>;
            ViewDetailUser_UserResponse resultData = actualResult?.Result;
            #endregion

            // Assert
            #region Assert
            Assert.NotNull(resultData);
            Assert.Equal(resultData.StaffCode, staffCode);
            Assert.Equal(resultData.UserName, expectedUser.UserName);
            Assert.Equal(resultData.Location, expectedUser.Location.ToString());
            #endregion
        }

        [Fact]
        public async Task GetSingleUser_InvalidStaffCode_BadRequest()
        {
            // Arrange 
            #region Arrange
            string staffCode = "Invalid";

            UserController userController = new UserController(_context, _userManager.Object, _mapper);
            List<AppUser> listUsers = _context.AppUsers.ToList();
            AppUser currentUser = listUsers.ElementAt(0);
            List<AppUser> addminRole = await _context.AppUsers
               .Where(x => !x.IsDeleted && x.UserName.Contains("admin"))
               .ToListAsync();
            List<AppUser> staffRole = await _context.AppUsers
               .Where(x => !x.IsDeleted && x.UserName.Contains("staff"))
               .ToListAsync();
            foreach (AppUser user in listUsers)
            {
                if (addminRole.Contains(user))
                    _userManager.Setup(_ => _.GetRolesAsync(user).Result).Returns(new List<string> { "Admin" });
                else
                    _userManager.Setup(_ => _.GetRolesAsync(user).Result).Returns(new List<string> { "Staff" });
            }
            #endregion

            // Act 
            #region Act
            var result = await userController.GetSingleUser(staffCode);
            var badrequestResult = result.Result as BadRequestObjectResult;
            ErrorResponseResult<string> actualResult = badrequestResult?.Value as ErrorResponseResult<string>;
            #endregion

            // Assert
            #region Assert
            Assert.NotNull(actualResult);
            Assert.Equal(400, badrequestResult.StatusCode);
            #endregion
        }
        #endregion

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
