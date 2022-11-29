using AssetManagement.Application.Controllers;
using AssetManagement.Contracts.AutoMapper;
using AssetManagement.Contracts.Common;
using AssetManagement.Contracts.User.Response;
using AssetManagement.Data.EF;
using AssetManagement.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AssetManagement.Application.Tests
{
    public class UserControllerTest
    {
        private readonly DbContextOptions _options;
        private readonly AssetManagementDbContext _context;
        private readonly IMapper _mapper;
        private UserManager<AppUser> _userManager;

        private readonly Guid _adminHCMId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE");
        private readonly Guid _staffId = new Guid("70BD714F-9576-45BA-B5B7-F00649BE00DE");
        private readonly Guid _wrongId = new Guid("70BD714F-9576-45DD-B5B7-DF3F325983DC");

        public UserControllerTest()
        {
            // Create InMemory dbcontext options
            _options = new DbContextOptionsBuilder<AssetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "AssetTestDb").Options;

            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new UserProfile())).CreateMapper();

            // Create InMemory dbcontext with options
            _context = new AssetManagementDbContext(_options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        #region Detele User
        [Fact]
        public async Task DeleteUser_Success()
        {
            var store = new Mock<IUserStore<AppUser>>();
            //store.Setup(x => x.FindByIdAsync("123", CancellationToken.None)).ReturnsAsync(new AppUser()
            //    {
            //        UserName = "test@email.com",
            //        Id = new Guid("8D04DCE2-969A-435D-BBB4-DF3F325983DC")
            //    });
            _userManager = new UserManager<AppUser>(store.Object, null, null, null, null, null, null, null, null);
            var controller = new UserController(_context, _userManager, _mapper);
            var id = _staffId;
            //controller.HttpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhZG1pbmhjbUBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9naXZlbm5hbWUiOiJUb2FuIEJhY2giLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW5oY20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTY2OTcyMzMxMywiaXNzIjoiMDEyMzQ1Njc4OUFCQ0RFRiIsImF1ZCI6IjAxMjM0NTY3ODlBQkNERUYifQ.J_t-YRZvRQOuZirjaC_lggwqtZW_SYa2-px4Id0YnW0";


            var claimsIdentity = new ClaimsIdentity(authenticationType: "test");
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, "adminhn"));
            var user = new ClaimsPrincipal(claimsIdentity);
            controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };


            var response = await controller.Delete(id);
            var result = ((OkObjectResult)response).Value;
            var responseId = ((DeleteUserResponse)result).Id;
            Assert.Equal(responseId, id);
        }

        [Fact]
        public async Task DeleteUser_Failed_NotExistUser()
        {
            var store = new Mock<IUserStore<AppUser>>();
            //store.Setup(x => x.FindByIdAsync("123", CancellationToken.None)).ReturnsAsync(new AppUser()
            //    {
            //        UserName = "test@email.com",
            //        Id = new Guid("8D04DCE2-969A-435D-BBB4-DF3F325983DC")
            //    });
            _userManager = new UserManager<AppUser>(store.Object, null, null, null, null, null, null, null, null);
            var controller = new UserController(_context, _userManager, _mapper);
            var id = _wrongId;
            //controller.HttpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhZG1pbmhjbUBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9naXZlbm5hbWUiOiJUb2FuIEJhY2giLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW5oY20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTY2OTcyMzMxMywiaXNzIjoiMDEyMzQ1Njc4OUFCQ0RFRiIsImF1ZCI6IjAxMjM0NTY3ODlBQkNERUYifQ.J_t-YRZvRQOuZirjaC_lggwqtZW_SYa2-px4Id0YnW0";


            var claimsIdentity = new ClaimsIdentity(authenticationType: "test");
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, "adminhn"));
            var user = new ClaimsPrincipal(claimsIdentity);
            controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var response = await controller.Delete(id);
            var result = ((BadRequestObjectResult)response).Value;

            var expected = new BadRequestObjectResult(new ErrorResponseResult<string>("Can't find this user"));

            Assert.Equal(((ErrorResponseResult<string>)expected.Value).Message, ((ErrorResponseResult<string>)result).Message);
        }

        [Fact]
        public async Task DeleteUser_Failed_DeleteSelf()
        {
            var store = new Mock<IUserStore<AppUser>>();
            //store.Setup(x => x.FindByIdAsync("123", CancellationToken.None)).ReturnsAsync(new AppUser()
            //    {
            //        UserName = "test@email.com",
            //        Id = new Guid("8D04DCE2-969A-435D-BBB4-DF3F325983DC")
            //    });
            _userManager = new UserManager<AppUser>(store.Object, null, null, null, null, null, null, null, null);
            var controller = new UserController(_context, _userManager, _mapper);
            var id = _adminHCMId;
            //controller.HttpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhZG1pbmhjbUBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9naXZlbm5hbWUiOiJUb2FuIEJhY2giLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW5oY20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTY2OTcyMzMxMywiaXNzIjoiMDEyMzQ1Njc4OUFCQ0RFRiIsImF1ZCI6IjAxMjM0NTY3ODlBQkNERUYifQ.J_t-YRZvRQOuZirjaC_lggwqtZW_SYa2-px4Id0YnW0";


            var claimsIdentity = new ClaimsIdentity(authenticationType: "test");
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, "adminhcm"));
            var user = new ClaimsPrincipal(claimsIdentity);
            controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var response = await controller.Delete(id);
            var result = ((BadRequestObjectResult)response).Value;

            var expected = new BadRequestObjectResult(new ErrorResponseResult<string>("You can't delete yourself"));

            Assert.Equal(((ErrorResponseResult<string>)expected.Value).Message, ((ErrorResponseResult<string>)result).Message);
        }
        #endregion
    }
}
