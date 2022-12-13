using AssetManagement.Application.Controllers;
using static AssetManagement.Application.Tests.TestHelper.ConverterFromIActionResult;
using AssetManagement.Contracts.AutoMapper;
using AssetManagement.Contracts.User.Request;
using AssetManagement.Data.EF;
using AssetManagement.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using AssetManagement.Contracts.User.Response;
using AssetManagement.Contracts.Common;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using AutoMapper.Internal;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace AssetManagement.Application.Tests
{
    public class UsersControllerTests : IAsyncDisposable
    {
        private readonly DbContextOptions _options;
        private readonly AssetManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly Mock<UserManager<AppUser>> _userManager;
        private List<AppRole> _roles;
        private List<AppUser> _users;

        public UsersControllerTests()
        {
            // Create InMemory dbcontext options
            _options = new DbContextOptionsBuilder<AssetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "UserTestDb").Options;

            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new UserProfile())).CreateMapper();

            //Create UserManager mock using userStoreMoq
            _userManager = new(new Mock<IUserStore<AppUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<AppUser>>().Object,
                new IUserValidator<AppUser>[0],
                new IPasswordValidator<AppUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<AppUser>>>().Object);

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
            Assert.Equal("Password is incorrect", message);
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
                .Where(
                    x => !x.IsDeleted &&
                    x.Location == currentUser.Location &&
                    (addminRole.Contains(x) || staffRole.Contains(x)))
                .OrderBy(x => x.StaffCode)
                .ToListAsync();

            var result = await userController.GetAllUser(0, 2, "", "", "staffCode", "ASC");
            var okobjectResult = result.Result as OkObjectResult;
            ViewListPageResult<ViewListUser_UserResponse> actualResult =
                okobjectResult.Value as ViewListPageResult<ViewListUser_UserResponse>;
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
            ViewListPageResult<ViewListUser_UserResponse> actualResult =
                okobjectResult.Value as ViewListPageResult<ViewListUser_UserResponse>;
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
            ViewListPageResult<ViewListUser_UserResponse> actualResult =
                okobjectResult.Value as ViewListPageResult<ViewListUser_UserResponse>;
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
            ViewListPageResult<ViewListUser_UserResponse> actualResult =
                okobjectResult.Value as ViewListPageResult<ViewListUser_UserResponse>;
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
            ViewListPageResult<ViewListUser_UserResponse> actualResult =
                okobjectResult.Value as ViewListPageResult<ViewListUser_UserResponse>;
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
            ViewListPageResult<ViewListUser_UserResponse> actualResult =
                okobjectResult.Value as ViewListPageResult<ViewListUser_UserResponse>;
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
            ViewListPageResult<ViewListUser_UserResponse> actualResult =
                okobjectResult.Value as ViewListPageResult<ViewListUser_UserResponse>;
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
                .Take(end > sortedListUser.Count() ? sortedListUser.Count() - start : end - start)
                .ToListAsync();


            var result = await userController.GetAllUser(start, end, "", "", "staffCode", "ASC");
            var okobjectResult = result.Result as OkObjectResult;
            ViewListPageResult<ViewListUser_UserResponse> actualResult =
                okobjectResult.Value as ViewListPageResult<ViewListUser_UserResponse>;
            #endregion

            // Assert
            #region Assert
            Assert.NotNull(actualResult);
            Assert.Equal(actualResult.Data.Count, expectedResult.Count);
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

        #region Create user
        [Fact]
        public async Task CreateUser_SuccessAsync()
        {
            //ARRANGE
            System.DateTime today = System.DateTime.Now;
            System.TimeSpan duration = new System.TimeSpan(157680, 0, 0, 0);
            CreateUserRequest userRequest = new()
            {
                FirstName = "Trong",
                LastName = "Nghia",
                Dob = new DateTime(1995, 12, 12),
                JoinedDate = new DateTime(2022, 12, 12),
                Gender = "0",
                Role = "admin"
            };
            var users = new List<AppUser>
            {
                new AppUser
                {
                    UserName = "Test",
                    Id = Guid.NewGuid(),
                    Email = "test@test.it"
                },
                 new AppUser
                {
                    UserName = "Test1234",
                    Id = Guid.NewGuid(),
                    Email = "test@test.it"
                }
            }.AsQueryable();

            _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(value: null);
            _userManager.Setup(x => x.Users)
                .Returns(users);
            _userManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
               .ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
              .ReturnsAsync(IdentityResult.Success);

            UserController controller = new(_context, _userManager.Object, _mapper);
            AppUser user = await _context.Users.FirstOrDefaultAsync();

            ClaimsIdentity _identity = new ClaimsIdentity();
            _identity.AddClaims(new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Locality, user.Location.ToString()),
            });

            //Create context for controller with fake login
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(_identity)
                }
            };

            //ACT
            var response = await controller.CreateUser(userRequest);
            var okResult = response as OkObjectResult;
            var actualResult = okResult.Value as CreateUserResponse;

            //ASSERT
            Assert.NotNull(response);
            Assert.NotNull(actualResult);
            Assert.IsType<OkObjectResult>(response);
            Assert.IsType<CreateUserResponse>(actualResult);
            Assert.Equal(userRequest.FirstName, actualResult.FirstName);
            Assert.Equal(userRequest.LastName, actualResult.LastName);
            Assert.Equal(userRequest.Dob, actualResult.Dob);
            Assert.Equal(userRequest.JoinedDate, actualResult.CreatedDate);
        }
        #endregion

        #region EditUser
        #nullable disable
        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        public async Task EditUser_SuccessAsync(int index)
        {
            //ARRANGE
            UpdateUserRequest request = new()
            {
                Dob = new(2000, 11, 28),
                Gender = (byte)Domain.Enums.AppUser.UserGender.Female,
                JoinedDate = new(2022, 11, 28),
                Type = "Admin"
            };

            string staffCode = _context.AppUsers.ToList()[index].StaffCode;

            UserController controller = new UserController(_context, _userManager.Object, _mapper);

            //ACT
            IActionResult result = await controller.UpdateUserAsync(staffCode, request);
            string data = ConvertOkObject<UpdateUserResponse>(result);
            UpdateUserResponse expected = _mapper.Map<UpdateUserResponse>(_context.AppUsers.ToList()[index]);
            expected.Type = "Admin";

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(data);
            Assert.Equal(JsonConvert.SerializeObject(expected), data);
        }

        [Fact]
        public async Task EditUser_BadRequest_UnderAgeAsync()
        {
            //ARRANGE
            UpdateUserRequest request = new()
            {
                Dob = DateTime.Now.AddYears(-18).AddSeconds(1),
                Gender = (byte)Domain.Enums.AppUser.UserGender.Female,
                JoinedDate = new(2022, 11, 28),
                Type = "Admin"
            };

            UserController controller = new UserController(_context, _userManager.Object, _mapper);

            //ACT
            IActionResult result = await controller.UpdateUserAsync("SD0001", request);
            string data = ConvertStatusCode(result);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("\"User is under 18. Please select a different date\"", data);
        }

        [Fact]
        public async Task EditUser_BadRequest_JoinedAgeAsync()
        {
            //ARRANGE
            UpdateUserRequest request = new()
            {
                Dob = new(2000, 11, 29),
                Gender = (byte)Domain.Enums.AppUser.UserGender.Female,
                JoinedDate = new(2018, 11, 28),
                Type = "Admin"
            };

            UserController controller = new UserController(_context, _userManager.Object, _mapper);

            //ACT
            IActionResult result = await controller.UpdateUserAsync("SD0001", request);
            string data = ConvertStatusCode(result);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("\"User under the age 18 may not join the company. Please select a different date\"", data);
        }

        [Theory]
        [InlineData(2022, 11, 27)]
        [InlineData(2022, 11, 26)]
        public async Task EditUser_BadRequest_JoinedWeekendAsync(int jyear, int jmonth, int jday)
        {
            //ARRANGE
            UpdateUserRequest request = new()
            {
                Dob = new(2000, 11, 20),
                Gender = (byte)Domain.Enums.AppUser.UserGender.Female,
                JoinedDate = new(jyear, jmonth, jday),
                Type = "Admin"
            };

            UserController controller = new UserController(_context, _userManager.Object, _mapper);

            //ACT
            IActionResult result = await controller.UpdateUserAsync("SD0001", request);
            string data = ConvertStatusCode(result);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("\"Joined date is Saturday or Sunday. Please select a different date\"", data);
        }

        [Fact]
        public async Task EditUser_BadRequest_NotFoundAsync()
        {
            //ARRANGE
            UpdateUserRequest request = new()
            {
                Dob = new(2000, 11, 29),
                Gender = (byte)Domain.Enums.AppUser.UserGender.Female,
                JoinedDate = new(2022, 11, 28),
                Type = "Admin"
            };

            UserController controller = new UserController(_context, _userManager.Object, _mapper);

            //ACT
            IActionResult result = await controller.UpdateUserAsync("INVAUR", request);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #region Delete user
        [Fact]
        public async Task DeleteUser_Failed_NotExistUser()
        {
            //ARRANGE
            var store = new Mock<IUserStore<AppUser>>();
            //store.Setup(x => x.FindByIdAsync("123", CancellationToken.None)).ReturnsAsync(new AppUser()
            //    {
            //        UserName = "test@email.com",
            //        Id = new Guid("8D04DCE2-969A-435D-BBB4-DF3F325983DC")
            //    });
            UserController controller = new UserController(_context, _userManager.Object, _mapper);
            string staffCode = "Invalid";
            //controller.HttpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhZG1pbmhjbUBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9naXZlbm5hbWUiOiJUb2FuIEJhY2giLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW5oY20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTY2OTcyMzMxMywiaXNzIjoiMDEyMzQ1Njc4OUFCQ0RFRiIsImF1ZCI6IjAxMjM0NTY3ODlBQkNERUYifQ.J_t-YRZvRQOuZirjaC_lggwqtZW_SYa2-px4Id0YnW0";


            var claimsIdentity = new ClaimsIdentity(authenticationType: "test");
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, "adminhn"));
            var user = new ClaimsPrincipal(claimsIdentity);
            controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            //ACT
            var response = await controller.Delete(staffCode);
            var result = ((BadRequestObjectResult)response).Value;

            var expected = new BadRequestObjectResult(new ErrorResponseResult<string>("Can't find this user"));
            //ASSERT
            Assert.Equal(((ErrorResponseResult<string>)expected.Value).Message, ((ErrorResponseResult<string>)result).Message);
        }

        [Fact]
        public async Task DeleteUser_Failed_DeleteSelf()
        {
            //ARRANGE
            var store = new Mock<IUserStore<AppUser>>();
            //store.Setup(x => x.FindByIdAsync("123", CancellationToken.None)).ReturnsAsync(new AppUser()
            //    {
            //        UserName = "test@email.com",
            //        Id = new Guid("8D04DCE2-969A-435D-BBB4-DF3F325983DC")
            //    });
            var controller = new UserController(_context, _userManager.Object, _mapper);
            string staffCode = "SD0001";
            //controller.HttpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhZG1pbmhjbUBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9naXZlbm5hbWUiOiJUb2FuIEJhY2giLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW5oY20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTY2OTcyMzMxMywiaXNzIjoiMDEyMzQ1Njc4OUFCQ0RFRiIsImF1ZCI6IjAxMjM0NTY3ODlBQkNERUYifQ.J_t-YRZvRQOuZirjaC_lggwqtZW_SYa2-px4Id0YnW0";


            var claimsIdentity = new ClaimsIdentity(authenticationType: "test");
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, "adminhcm"));
            var user = new ClaimsPrincipal(claimsIdentity);
            controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            //ACT
            var response = await controller.Delete(staffCode);
            var result = ((BadRequestObjectResult)response).Value;

            var expected = new BadRequestObjectResult(new ErrorResponseResult<string>("You can't delete yourself"));
            //ASSERT
            Assert.Equal(((ErrorResponseResult<string>)expected.Value).Message, ((ErrorResponseResult<string>)result).Message);
        }

        [Fact]
        public async Task DeleteUser_Failed_AvailableAssignment()
        {
            //ARRANGE
            UserController controller = new UserController(_context, _userManager.Object, _mapper);
            string staffCode = "SD0003";
            //Add login
            var claimsIdentity = new ClaimsIdentity(authenticationType: "test");
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, "adminhcm"));
            var user = new ClaimsPrincipal(claimsIdentity);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            //ACT
            IActionResult response = await controller.Delete(staffCode);
            ErrorResponseResult<string> result = (ErrorResponseResult<string>)((ObjectResult) response).Value;
            //ASSERT
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.False(result.IsSuccessed);
            Assert.Equal("There are valid assignments belonging to this user. Please close all assignments before disabling user.", result.Message);
        }

        [Fact]
        public async Task DeleteUser_Success()
        {
            //ARRANGE
            UserController controller = new UserController(_context, _userManager.Object, _mapper);
            AppUser removable = new()
            {
                Id = new Guid("8DAAAAE2-9BBA-4CCD-BDD4-DFEF325FF3FF"),
                UserName = "removable",
                NormalizedUserName = "removable",
                Email = "removable@gmail.com",
                NormalizedEmail = "removable@gmail.com",
                EmailConfirmed = true,
                PasswordHash = "123abc",
                SecurityStamp = string.Empty,
                FirstName = "Toan",
                LastName = "Bach",
                Dob = new DateTime(2000, 11, 11),
                IsLoginFirstTime = true,
                CreatedDate = DateTime.Now,
                Gender = Domain.Enums.AppUser.UserGender.Male,
                Location = Domain.Enums.AppUser.AppUserLocation.HoChiMinh,
                StaffCode = "SD9999"
            };
            await _context.AppUsers.AddAsync(removable);
            await _context.UserRoles.AddAsync(new()
            {
                RoleId = new("12147FE0-4571-4AD2-B8F7-D2C863EB78A5"),
                UserId = removable.Id
            });
            await _context.SaveChangesAsync();
            string staffCode = "SD9999";
            //Add login
            var claimsIdentity = new ClaimsIdentity(authenticationType: "test");
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, "adminhcm"));
            var user = new ClaimsPrincipal(claimsIdentity);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            //ACT
            IActionResult response = await controller.Delete(staffCode);
            DeleteUserResponse result = (DeleteUserResponse)((ObjectResult)response).Value;
            //Assert
            Assert.IsType<OkObjectResult>(response);
            Asset.Equals(staffCode, result.StaffCode);
        }
        #endregion

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await _context.Database.CloseConnectionAsync();
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }
    }
}
