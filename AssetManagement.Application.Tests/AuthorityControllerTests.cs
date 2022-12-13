using AssetManagement.Application.Controllers;
using AssetManagement.Contracts.Authority.Request;
using AssetManagement.Contracts.Authority.Response;
using AssetManagement.Contracts.AutoMapper;
using AssetManagement.Contracts.Common;
using AssetManagement.Data.EF;
using AssetManagement.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Security.Principal;
using Xunit;

#nullable disable
namespace AssetManagement.Application.Tests
{
    public class AuthorityControllerTests : IAsyncDisposable
    {
        private readonly DbContextOptions _options;
        private readonly AssetManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly Mock<UserManager<AppUser>> _userManager;

        public AuthorityControllerTests()
        {
            //Create InMemory dbcontext options
            _options = new DbContextOptionsBuilder<AssetManagementDbContext>().UseInMemoryDatabase("AuthTestDB").Options;
            //Create InMemory dbcontext with options
            _context = new AssetManagementDbContext(_options);
            //Create mapper using UserProfile
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new UserProfile())).CreateMapper();
            //Create fake config with fake jwt settings
            Dictionary<string, string> inMemorySettings = new()  {
                {"JwtSettings:validIssuer", "issuer"},
                {"JwtSettings:expires", "1"},
                {"JwtSettings:Key", "!^##7w7$3tt!n9Key##^!" }
            };
            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            //Mock UserManager
            //Create UserStore mock to enable user support for UserManager
            Mock<IUserStore<AppUser>> userStoreMoq = new();
            //Create UserManager mock using userStoreMoq
            _userManager = new(userStoreMoq.Object, null, null, null, null, null, null, null, null);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        //Tests
        [Theory]
        [InlineData("adminhcm")]
        [InlineData("adminhn")]
        public void GetUserProfile_Success(string username)
        {
            //ARRANGE
            AppUser user = _context.AppUsers.FirstOrDefault(u => u.UserName == username);
            //Set up UserManager, assume that user is stored
            _userManager.Setup(um => um.FindByNameAsync(username))
                        .ReturnsAsync(user);
            //Create controller
            AuthorityController controller = new AuthorityController(_userManager.Object, _config, _context, _mapper);
            //Create context for controller with fake login
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(username), null)
                }
            };

            //ACT
            //Get current login profile
            IActionResult result = controller.GetUserProfile().Result;

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equivalent(_mapper.Map<UserResponse>(user), ((OkObjectResult)result).Value);
        }

        [Fact]
        public void Authenticate_Success()
        {
            //ARRANGE
            //Create login request
            LoginRequest request = new() { Username = "adminhcm", Password = "12345678" };
            AppUser user = _context.AppUsers.FirstOrDefault(u => u.UserName == request.Username);
            //Set up UserManager, assume that login request is correct
            _userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManager.Setup(um => um.CheckPasswordAsync(user, request.Password)).ReturnsAsync(true);
            _userManager.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string>{ "Admin, Staff"});
            //Create controller
            AuthorityController controller = new AuthorityController(_userManager.Object, _config, _context, _mapper);

            //ACT
            IActionResult result = controller.Authenticate(request).Result;

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData(null, "123")]
        [InlineData("binhnv", null)]
        [InlineData(null, null)]
        public void Authenticate_BadRequest_ModelState_Invalid(string username, string password)
        {
            //ARRANGE
            LoginRequest request = new() { Username = username, Password = password };
            //Create controller
            AuthorityController controller = new AuthorityController(_userManager.Object, _config, _context, _mapper);
            if (username == null) controller.ModelState.AddModelError("username", "Please enter username");
            if (password == null) controller.ModelState.AddModelError("password", "Please enter password");

            //ACT
            IActionResult result = controller.Authenticate(request).Result;
            SerializableError errors = (SerializableError)((ObjectResult)result).Value;

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            if (errors != null && errors.ContainsKey("username"))
            {
                string message = (errors["username"] as string[]).FirstOrDefault();
                Assert.Equal("Please enter username", message);
            }
            if (errors != null && errors.ContainsKey("password"))
            {
                string message = (errors["password"] as string[]).FirstOrDefault();
                Assert.Equal("Please enter password", message);
            }
        }

        [Theory]
        [InlineData("thanhnv")]
        [InlineData("datnv")]
        public void Authenticate_BadRequest_Username(string username)
        {
            // ARRANGE
            //Create login request (no need password)
            LoginRequest request = new() { Username = username };
            //Set up UserManager
            _userManager.Setup(um => um.FindByNameAsync(username))
                        .ReturnsAsync(_context.AppUsers.FirstOrDefault(u => u.UserName == username));

            AuthorityController controller = new AuthorityController(_userManager.Object, _config, _context, _mapper);
            //ACT
            IActionResult result = controller.Authenticate(request).Result;
            string message = ((ErrorResponseResult<string>)((ObjectResult)result).Value).Message;

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Username or password is incorrect. Please try again", message);
        }

        [Theory]
        [InlineData("adminhcm", "asdkjg")]
        [InlineData("adminhn", "qwerty")]
        public void Authenticate_BadRequest_Password(string username, string passwordHash)
        {
            // ARRANGE
            //Create login request (no need password)
            LoginRequest request = new() { Username = username };
            AppUser user = _context.AppUsers.FirstOrDefault(u => u.UserName == username);
            //Set up UserManager
            _userManager.Setup(um => um.FindByNameAsync(username)).ReturnsAsync(user);
            _userManager.Setup(um => um.CheckPasswordAsync(user, passwordHash))
                        .ReturnsAsync(user.PasswordHash == passwordHash);

            AuthorityController controller = new AuthorityController(_userManager.Object, _config, _context, _mapper);

            //ACT
            IActionResult result = controller.Authenticate(request).Result;
            string message = ((ErrorResponseResult<string>)((ObjectResult)result).Value).Message;

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Username or password is incorrect. Please try again", message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void ChangePassword_Success(int index)
        {
            // ARRANGE
            AppUser user = _context.Users.ToList()[index];
            //Create change password request
            ChangePasswordRequest request = new()
            {
                CurrentPassword = user.PasswordHash,
                NewPassword = "qwe",
                ConfirmPassword = "qwe"
            };

            //Set up UserManager
            _userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManager.Setup(um => um.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword))
                        .ReturnsAsync(IdentityResult.Success);

            //Create context for controller with fake login
            AuthorityController controller = new AuthorityController(_userManager.Object, _config, _context, _mapper);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(user.UserName), null)
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

        //Clean up after tests

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await _context.Database.CloseConnectionAsync();
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }
    }
}