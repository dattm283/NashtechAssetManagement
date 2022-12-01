﻿using AssetManagement.Application.Controllers;
using AssetManagement.Contracts.Asset.Request;
using AssetManagement.Contracts.AutoMapper;
using AssetManagement.Contracts.Category.Request;
using AssetManagement.Contracts.Common;
using AssetManagement.Contracts.User.Request;
using AssetManagement.Data.EF;
using AssetManagement.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AssetManagement.Application.Tests
{
    public class UsersControllerTests : IDisposable
    {
        private readonly DbContextOptions _options;
        private readonly AssetManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly Mock<UserManager<AppUser>> _userManager;
        private List<AppRole> _roles;
        private List<AppUser> _users;

        public UsersControllerTests()
        {
            //Create InMemory dbcontext options
            _options = new DbContextOptionsBuilder<AssetManagementDbContext>().UseInMemoryDatabase("UserTestDB").Options;
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
            //Create fake data
            SeedData();
        }

        #region Change User Password
        [Theory]
        [InlineData("12345678", "123456")]
        //[InlineData("12345678", "1234567")]
        public async Task UserChangePassword_SuccessAsync(string currentpassword, string newpassword)
        {
            //ARRANGE
            UserChangePasswordRequest request = new() { CurrentPassword = currentpassword, NewPassword = newpassword };

            //Set up UserManager
            _userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(_users[0]);
            _userManager.Setup(um => um.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(true));
            _userManager.Setup(um => um.ChangePasswordAsync(_users[0], currentpassword, newpassword))
                        .ReturnsAsync(IdentityResult.Success);

            UserController controller = new UserController(_userManager.Object, _config, _context, _mapper);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(_users[0].UserName), null)
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

        [Theory]
        [InlineData("123456", "123456")]
        public async Task UserChangePassword_WrongPassword_ReturnBadRequest(string currentpassword, string newpassword)
        {
            UserChangePasswordRequest request = new() { CurrentPassword = currentpassword, NewPassword = newpassword };

            //Set up UserManager
            _userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(_users[0]);
            _userManager.Setup(um => um.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(false));

            UserController controller = new UserController(_userManager.Object, _config, _context, _mapper);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(_users[0].UserName), null)
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

        [Theory]
        [InlineData("12345678", "12345678")]
        public async Task UserChangePassword_NewPasswordIsOldPassword_ReturnBadRequest(string currentpassword, string newpassword)
        {
            UserChangePasswordRequest request = new() { CurrentPassword = currentpassword, NewPassword = newpassword };

            //Set up UserManager
            _userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(_users[0]);
            _userManager.Setup(um => um.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(false));

            UserController controller = new UserController(_userManager.Object, _config, _context, _mapper);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(_users[0].UserName), null)
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

        //Create InMemory Data
        private void SeedData()
        {
            //Make sure InMemory data is deleted before creating new data
            //To avoid duplicated keys error
            _context.Database.EnsureDeleted();
            //Create roles data
            var hasher = new PasswordHasher<AppUser>();
            _roles = new()
            {
                new AppRole()
                {
                    Id = new Guid(1, 2, 3, 4, 4, 4, 4, 4, 4, 4, 4),
                    Name = "Admin",
                    Description = "Admin role"
                },

                new AppRole()
                {
                    Id = new Guid(1, 2, 3, 4, 4, 4, 4, 4, 4, 4, 5),
                    Name = "Staff",
                    Description = "Staff role"
                }
            };
            //Create users data
            _users = new()
            {
                new AppUser()
                {
                    Id = new Guid(1, 2, 3, 4, 4, 4, 4, 4, 4, 4, 7),
                    FirstName = "Binh",
                    LastName = "Nguyen Van",
                    UserName = "binhnv",
                    Email = "bnv@gmail.com",
                    PasswordHash = "12345678",
                    Gender = Domain.Enums.AppUser.UserGender.Male,
                    Location = Domain.Enums.AppUser.AppUserLocation.HoChiMinh,
                    //RoleId = _roles[0].Id,
                    IsLoginFirstTime = true,
                    StaffCode = "SD01",
                },

                new AppUser()
                {
                    FirstName = "An",
                    LastName = "Nguyen Van",
                    UserName = "annv",
                    Email = "anv@gmail.com",
                    PasswordHash = hasher.HashPassword(null, "12345678"),
                    Gender = Domain.Enums.AppUser.UserGender.Male,
                    Location = Domain.Enums.AppUser.AppUserLocation.HaNoi,
                    //RoleId = _roles[0].Id,
                    IsLoginFirstTime = true,
                    StaffCode = "SD02",
                }
            };
            //Add roles
            _context.AppRoles.AddRange(_roles);
            //Add users
            _context.AppUsers.AddRange(_users);
            _context.UserRoles.Add(new IdentityUserRole<Guid>
            {
                RoleId = _roles.First().Id,
                UserId = _users.First().Id,
            });
            _context.SaveChanges();
        }

        //Clean up after tests
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
