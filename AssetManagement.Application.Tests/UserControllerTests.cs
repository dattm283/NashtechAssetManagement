using AssetManagement.Application.Controllers;
using AssetManagement.Contracts.Asset.Response;
using AssetManagement.Contracts.AutoMapper;
using AssetManagement.Data.EF;
using AssetManagement.Domain.Models;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Xunit;
using Microsoft.AspNetCore.Identity;
using AssetManagement.Contracts.User.Response;
using Moq;
using System.Linq;
using AssetManagement.Domain.Enums.AppUser;
using AssetManagement.Contracts.Common;
using AssetManagement.Contracts.Asset.Request;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using AssetManagement.Contracts.User.Request;
using Microsoft.Extensions.Options;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Logging;
using static AssetManagement.Application.Tests.UserControllerTests;
using System.Collections.Generic;
using static AssetManagement.Application.Tests.TestHelper.ConverterFromIActionResult;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using Microsoft.Extensions.DependencyInjection;
using AssetManagement.Application.Tests.TestHelper;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;

namespace AssetManagement.Application.Tests
{
    public class UserControllerTests : IDisposable
    {
        private readonly Mock<UserManager<AppUser>> _userManager;
        private readonly DbContextOptions _options;
        private readonly AssetManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private List<AppUser> _users;
        private List<Asset> _assets;
        private List<Category> _categories;

        public UserControllerTests()
        {
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


        #region GetUser
        [Fact]
        public async Task GetList_ForDefault_Ok()
        {
            // Arrange 
            #region Arrange
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

            var result = await userController.GetAllUser(0, 2, "", "", "staffCode", "ASC", currentUser.UserName);
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
        public async Task GetList_ForDefault_BadRequest_InvalidUserName()
        {
            // Arrange 
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
            _userManager.Setup(_ => _.GetUsersInRoleAsync("Admin").Result).Returns(addminRole);
            _userManager.Setup(_ => _.GetUsersInRoleAsync("Staff").Result).Returns(staffRole);

            // Act 

            var result = await userController.GetAllUser(0, 2, "", "", "staffCode", "ASC", "");
            var badrequestResult = result.Result as BadRequestObjectResult;
            string actualResult = badrequestResult?.Value as string;

            // Assert
            Assert.NotNull(actualResult);
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

            var result = await userController.GetAllUser(0, 2, "", searchString, "staffCode", "ASC", currentUser.UserName);
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
            string filterState = "0&1&";
            var listType = filterState.Split("&").ToArray();

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
            _userManager.Setup(_ => _.GetUsersInRoleAsync("Admin").Result).Returns(addminRole);
            _userManager.Setup(_ => _.GetUsersInRoleAsync("Staff").Result).Returns(staffRole);
            #endregion

            // Act 
            #region Act
            List<AppUser> tempData = new List<AppUser>();
            for (int i = 0; i < listType.Length - 1; i++)
            {
                List<AppUser> tempUser = (listType[i] == "0") ? addminRole : staffRole;
                tempData.AddRange(tempUser);
            }
            List<AppUser> expectedResult = await _context.AppUsers
                .Where(x =>
                    !x.IsDeleted &&
                    x.Location == currentUser.Location &&
                    tempData.Contains(x))
                .OrderBy(x => x.StaffCode)
                .ToListAsync();

            var result = await userController.GetAllUser(0, 2, filterState, "", "staffCode", "ASC", currentUser.UserName);
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
        public async Task GetList_Sort_Ok()
        {
            // Arrange 
            #region Arrange
            string sort = "userName";

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

            var result = await userController.GetAllUser(0, 2, "", "", sort, "ASC", currentUser.UserName);
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


            var result = await userController.GetAllUser(start, end, "", "", "staffCode", "ASC", currentUser.UserName);
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
            ViewDetailUser_UserResponse actualResult =
                okobjectResult.Value as ViewDetailUser_UserResponse;
            #endregion

            // Assert
            #region Assert
            Assert.NotNull(actualResult);
            Assert.Equal(actualResult.StaffCode, staffCode);
            Assert.Equal(actualResult.UserName, expectedUser.UserName);
            Assert.Equal(actualResult.Location, expectedUser.Location.ToString());
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
                JoinedDate = today.Add(new TimeSpan(157680, 0, 0, 0)),
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

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
