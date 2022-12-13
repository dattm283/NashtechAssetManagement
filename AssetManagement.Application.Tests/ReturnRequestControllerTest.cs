using AssetManagement.Application.Controllers;
using AssetManagement.Contracts.Common;
using AssetManagement.Contracts.AutoMapper;
using AssetManagement.Data.EF;
using AssetManagement.Domain.Models;
using AutoMapper;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;
using static AssetManagement.Application.Tests.TestHelper.ConverterFromIActionResult;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using AssetManagement.Contracts.ReturnRequest.Response;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;

namespace AssetManagement.Application.Tests
{
    public class ReturnRequestControllerTest: IAsyncDisposable
    {
        private readonly DbContextOptions _options;
        private readonly AssetManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;

        public ReturnRequestControllerTest()
        {
            // Create InMemory dbcontext options
            _options = new DbContextOptionsBuilder<AssetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "AssignmentTestDb").Options;

            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new ReturnRequestProfile())).CreateMapper();

            // Create InMemory dbcontext with options
            _context = new AssetManagementDbContext(_options);
            _context.Database.EnsureDeleted();
            //SeedData();
            _context.Database.EnsureCreated();
        }

        #region GetList
        [Fact]
        public async Task GetList_ForDefault()
        {
            // Arrange 
            ReturnRequestController returnRequestController = new ReturnRequestController(_context, _mapper);

            // Act 
            var result = await returnRequestController.Get(1, 2);

            var list = _context.ReturnRequests
                .Include(x => x.AssignedByUser)
                .Include(x => x.AcceptedByUser)
                .Include(x => x.Assignment)
                    .ThenInclude(a => a.Asset)
                .Where(x => !x.IsDeleted)
                .Select(x => new ViewListReturnRequestResponse
                {
                    Id = x.Id,
                    NoNumber = x.Id,
                    AssetCode = x.Assignment.Asset.AssetCode,
                    AssetName = x.Assignment.Asset.Name,
                    RequestedBy = x.AssignedByUser.UserName,
                    AcceptedBy = x.AcceptedByUser.UserName,
                    AssignedDate = x.AssignedDate,
                    ReturnedDate = x.ReturnedDate,
                    State = x.State,
                })
                .OrderBy(x => x.Id);

            var expected = StaticFunctions<ViewListReturnRequestResponse>.Paging(list, 1, 2);

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewListPageResult<ViewListReturnRequestResponse>)okobjectResult.Value;

            var assignmentsList = resultValue.Data;

            var isSorted = assignmentsList.SequenceEqual(expected);
            // Assert
            Assert.True(isSorted);
            Assert.Equal(assignmentsList.Count(), expected.Count());
        }

        [Fact]
        public async Task GetList_SearchString_WithData()
        {
            // Arrange 
            ReturnRequestController returnRequestController = new ReturnRequestController(_context, _mapper);
            var searchString = "SD";
            // Act 
            var result = await returnRequestController.Get(1, 2, searchString);

            var list = _context.ReturnRequests
                .Include(x => x.AssignedByUser)
                .Include(x => x.AcceptedByUser)
                .Include(x => x.Assignment)
                    .ThenInclude(a => a.Asset)
                .Where(x => !x.IsDeleted && (x.Assignment.Asset.Name.Contains(searchString) ||
                    x.Assignment.Asset.AssetCode.Contains(searchString) ||
                    x.AssignedByUser.UserName.Contains(searchString)))
                .Select(x => new ViewListReturnRequestResponse
                {
                    Id = x.Id,
                    NoNumber = x.Id,
                    AssetCode = x.Assignment.Asset.AssetCode,
                    AssetName = x.Assignment.Asset.Name,
                    RequestedBy = x.AssignedByUser.UserName,
                    AcceptedBy = x.AcceptedByUser.UserName,
                    AssignedDate = x.AssignedDate,
                    ReturnedDate = x.ReturnedDate,
                    State = x.State,
                })
                .OrderBy(x => x.Id);

            var expected = JsonConvert.SerializeObject(StaticFunctions<ViewListReturnRequestResponse>.Paging(list, 1, 2));

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewListPageResult<ViewListReturnRequestResponse>)okobjectResult.Value;

            var assignmentsList = JsonConvert.SerializeObject(resultValue.Data);

            var isSorted = assignmentsList.SequenceEqual(expected);
            // Assert
            Assert.True(isSorted);
            Assert.Equal(assignmentsList.Count(), expected.Count());
        }

        [Fact]
        public async Task GetList_SearchString_WithOutData()
        {
            // Arrange 
            ReturnRequestController returnRequestController = new ReturnRequestController(_context, _mapper);
            var searchString = "9haha blabla";
            // Act 
            var result = await returnRequestController.Get(1, 2, searchString);

            var list = _context.ReturnRequests
                .Include(x => x.AssignedByUser)
                .Include(x => x.AcceptedByUser)
                .Include(x => x.Assignment)
                    .ThenInclude(a => a.Asset)
                .Where(x => !x.IsDeleted && (x.Assignment.Asset.Name.Contains(searchString) ||
                    x.Assignment.Asset.AssetCode.Contains(searchString) ||
                    x.AssignedByUser.UserName.Contains(searchString)))
                .Select(x => new ViewListReturnRequestResponse
                {
                    Id = x.Id,
                    NoNumber = x.Id,
                    AssetCode = x.Assignment.Asset.AssetCode,
                    AssetName = x.Assignment.Asset.Name,
                    RequestedBy = x.AssignedByUser.UserName,
                    AcceptedBy = x.AcceptedByUser.UserName,
                    AssignedDate = x.AssignedDate,
                    ReturnedDate = x.ReturnedDate,
                    State = x.State,
                })
                .OrderBy(x => x.Id);

            var expected = StaticFunctions<ViewListReturnRequestResponse>.Paging(list, 1, 2);

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewListPageResult<ViewListReturnRequestResponse>)okobjectResult.Value;

            var assignmentsList = resultValue.Data;

            var isSorted = assignmentsList.SequenceEqual(expected);
            // Assert
            Assert.True(isSorted);
            Assert.Equal(assignmentsList.Count(), expected.Count());
        }

        [Fact]
        public async Task GetList_FilterState()
        {
            // Arrange 
            ReturnRequestController returnRequestController = new ReturnRequestController(_context, _mapper);
            var state = (int)AssetManagement.Domain.Enums.Assignment.State.Accepted;
            // Act 
            var result = await returnRequestController.Get(1, 2, stateFilter: state.ToString());

            var list = _context.ReturnRequests
                .Include(x => x.AssignedByUser)
                .Include(x => x.AcceptedByUser)
                .Include(x => x.Assignment)
                    .ThenInclude(a => a.Asset)
                .Where(x => !x.IsDeleted && (int)x.State == state)
                .Select(x => new ViewListReturnRequestResponse
                {
                    Id = x.Id,
                    NoNumber = x.Id,
                    AssetCode = x.Assignment.Asset.AssetCode,
                    AssetName = x.Assignment.Asset.Name,
                    RequestedBy = x.AssignedByUser.UserName,
                    AcceptedBy = x.AcceptedByUser.UserName,
                    AssignedDate = x.AssignedDate,
                    ReturnedDate = x.ReturnedDate,
                    State = x.State,
                })
                .OrderBy(x => x.Id);

            var expected = JsonConvert.SerializeObject(
                StaticFunctions<ViewListReturnRequestResponse>.Paging(list, 1, 2));

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewListPageResult<ViewListReturnRequestResponse>)okobjectResult.Value;

            var assignmentsList = JsonConvert.SerializeObject(resultValue.Data);

            Assert.Equal(expected, assignmentsList);
        }

        [Fact]
        public async Task GetList_FilterReturnedDate()
        {
            // Arrange 
            ReturnRequestController returnRequestController = new ReturnRequestController(_context, _mapper);
            var returnedDateFilter = "2022-12-09";
            // Act 
            var result = await returnRequestController.Get(1, 2, returnedDateFilter: returnedDateFilter);

            var list = _context.ReturnRequests
                .Include(x => x.AssignedByUser)
                .Include(x => x.AcceptedByUser)
                .Include(x => x.Assignment)
                    .ThenInclude(a => a.Asset)
                .Where(x => !x.IsDeleted &&
                    x.ReturnedDate.Value.Date == DateTime.Parse(returnedDateFilter).Date)
                .Select(x => new ViewListReturnRequestResponse
                {
                    Id = x.Id,
                    NoNumber = x.Id,
                    AssetCode = x.Assignment.Asset.AssetCode,
                    AssetName = x.Assignment.Asset.Name,
                    RequestedBy = x.AssignedByUser.UserName,
                    AcceptedBy = x.AcceptedByUser.UserName,
                    AssignedDate = x.AssignedDate,
                    ReturnedDate = x.ReturnedDate,
                    State = x.State,
                })
                .OrderBy(x => x.Id);

            var expected = JsonConvert.SerializeObject(
                StaticFunctions<ViewListReturnRequestResponse>.Paging(list, 1, 2));

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewListPageResult<ViewListReturnRequestResponse>)okobjectResult.Value;

            var assignmentsList = JsonConvert.SerializeObject(resultValue.Data);

            Assert.Equal(expected, assignmentsList);
        }

        [Fact]
        public async Task GetList_ForDefaultSortedByAssetCode()
        {
            // Arrange 
            ReturnRequestController returnRequestController = new ReturnRequestController(_context, _mapper);
            var sortType = "assetCode";
            // Act 
            var result = await returnRequestController.Get(1, 2, sort: sortType);

            var list = _context.ReturnRequests
                .Include(x => x.AssignedByUser)
                .Include(x => x.AcceptedByUser)
                .Include(x => x.Assignment)
                    .ThenInclude(a => a.Asset)
                .Where(x => !x.IsDeleted)
                .Select(x => new ViewListReturnRequestResponse
                {
                    Id = x.Id,
                    NoNumber = x.Id,
                    AssetCode = x.Assignment.Asset.AssetCode,
                    AssetName = x.Assignment.Asset.Name,
                    RequestedBy = x.AssignedByUser.UserName,
                    AcceptedBy = x.AcceptedByUser.UserName,
                    AssignedDate = x.AssignedDate,
                    ReturnedDate = x.ReturnedDate,
                    State = x.State,
                })
                .OrderBy(x => x.AssetCode);

            var expected = JsonConvert.SerializeObject(
                StaticFunctions<ViewListReturnRequestResponse>.Paging(list, 1, 2));

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewListPageResult<ViewListReturnRequestResponse>)okobjectResult.Value;

            var assignmentsList = JsonConvert.SerializeObject(resultValue.Data);

            Assert.Equal(expected, assignmentsList);
        }

        [Fact]
        public async Task GetList_ForDefault_InvalidPaging()
        {
            // Arrange 
            ReturnRequestController returnRequestController = new ReturnRequestController(_context, _mapper);

            // Act 
            var result = await returnRequestController.Get(-1, 2);

            var list = _context.ReturnRequests
                .Include(x => x.AssignedByUser)
                .Include(x => x.AcceptedByUser)
                .Include(x => x.Assignment)
                    .ThenInclude(a => a.Asset)
                .Where(x => !x.IsDeleted)
                .Select(x => new ViewListReturnRequestResponse
                {
                    Id = x.Id,
                    NoNumber = x.Id,
                    AssetCode = x.Assignment.Asset.AssetCode,
                    AssetName = x.Assignment.Asset.Name,
                    RequestedBy = x.AssignedByUser.UserName,
                    AcceptedBy = x.AcceptedByUser.UserName,
                    AssignedDate = x.AssignedDate,
                    ReturnedDate = x.ReturnedDate,
                    State = x.State,
                })
                .OrderBy(x => x.Id);

            var expected = StaticFunctions<ViewListReturnRequestResponse>.Paging(list, -1, 2);

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewListPageResult<ViewListReturnRequestResponse>)okobjectResult.Value;

            var assignmentsList = resultValue.Data;

            var isSorted = assignmentsList.SequenceEqual(expected);
            // Assert
            Assert.True(isSorted);
            Assert.Equal(assignmentsList.Count(), expected.Count());
        }
        #endregion

        #region Create Returning Request
        [Fact]
        public async Task CreateReturnRequest_InvalidUser()
        {
            // Arrange
            int Id = 1;

            ReturnRequestController returnRequest = new ReturnRequestController(_context, _mapper);
            List<AppUser> listUsers = _context.AppUsers.ToList();
            AppUser currentUser = listUsers.ElementAt(1);
            currentUser.UserName = "Invalid User";
            returnRequest.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(currentUser.UserName), null)
                }
            };

            Assignment expectedAssignment = _context.Assignments.FirstOrDefault(x => x.Id == Id);

            // Act
            var objectResult = await returnRequest.CreateReturnRequest(Id);
            var okObjectResult = objectResult as BadRequestObjectResult;
            ErrorResponseResult<string> ok_data = okObjectResult.Value as ErrorResponseResult<string>;

            // Assert
            Assert.NotNull(ok_data);
            Assert.Equal(ok_data.Message, "Invalid User");
            //Assert.Equal((int)expectedAssignment.State, (int)Domain.Enums.Assignment.State.WaitingForReturning);
            //Assert.Equal(_context.ReturnRequests.LastOrDefault().State, Domain.Enums.ReturnRequest.State.WaitingForReturning);
        }

        [Fact]
        public async Task CreateReturnRequest_InvalidAssignment()
        {
            // Arrange
            int Id = 999;

            ReturnRequestController returnRequest = new ReturnRequestController(_context, _mapper);
            List<AppUser> listUsers = _context.AppUsers.ToList();
            AppUser currentUser = listUsers.ElementAt(1);
            returnRequest.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(currentUser.UserName), null)
                }
            };

            Assignment expectedAssignment = _context.Assignments.FirstOrDefault(x => x.Id == Id);

            // Act
            var objectResult = await returnRequest.CreateReturnRequest(Id);
            var okObjectResult = objectResult as BadRequestObjectResult;
            ErrorResponseResult<string> ok_data = okObjectResult.Value as ErrorResponseResult<string>;

            // Assert
            Assert.NotNull(ok_data);
            Assert.Equal(ok_data.Message, "Invalid Assignment");
            //Assert.Equal((int)expectedAssignment.State, (int)Domain.Enums.Assignment.State.WaitingForReturning);
            //Assert.Equal(_context.ReturnRequests.LastOrDefault().State, Domain.Enums.ReturnRequest.State.WaitingForReturning);
        }

        [Fact]
        public async Task CreateReturnRequest_Success()
        {
            // Arrange
            int Id = 1;

            ReturnRequestController returnRequest = new ReturnRequestController(_context, _mapper);
            List<AppUser> listUsers = _context.AppUsers.ToList();
            AppUser currentUser = listUsers.ElementAt(1);
            returnRequest.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(currentUser.UserName), null)
                }
            };

            Assignment expectedAssignment = _context.Assignments.FirstOrDefault(x => x.Id == Id);

            // Act
            var objectResult = await returnRequest.CreateReturnRequest(Id);
            var okObjectResult = objectResult as OkObjectResult;
            SuccessResponseResult<string> ok_data = okObjectResult.Value as SuccessResponseResult<string>;

            // Assert
            Assert.NotNull(ok_data);
            Assert.Equal(ok_data.Result, "Create ReturningRequest successfully");
            Assert.Equal((int)expectedAssignment.State, (int)Domain.Enums.Assignment.State.WaitingForReturning);
            Assert.Equal(_context.ReturnRequests.LastOrDefault().State, Domain.Enums.ReturnRequest.State.WaitingForReturning);
        }
        #endregion

        #region CancelReturnRequest
        [Fact]
        public async Task CancelReturnRequest_Success_ReturnDeletedAsset()
        {
            // Arrange 
            ReturnRequestController returnRequestController = new ReturnRequestController(_context, _mapper);
            var canceledRequest = _mapper
                .Map<CancelReturnRequestResponse>(await _context.ReturnRequests
                .Include(a => a.Assignment)
                .Where(a => a.Id == 1 && !a.IsDeleted &&
                    a.State == Domain.Enums.ReturnRequest.State.WaitingForReturning)
                .FirstOrDefaultAsync());
            //canceledRequest = Domain.Enums.Assignment.State.Accepted;

            // Act 
            var result = await returnRequestController.CancelReturnRequest(1);

            string resultObject = ConvertOkObject<CancelReturnRequestResponse>(result);
            string expectedObject = JsonConvert.SerializeObject(canceledRequest);

            // Assert
            Assert.Equal(resultObject, expectedObject);
        }

        [Fact]
        public async Task CancelReturnRequest_Invalid_ReturnBadRequest()
        {
            // Arrange 
            ReturnRequestController returnRequestController = new ReturnRequestController(_context, _mapper);

            // Act 
            var result = await returnRequestController.CancelReturnRequest(0);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();

        }
        #endregion

        #region ReturnRequest
        [Fact]
        public async Task ReturnRequest_SuccessAsync()
        {
            // Arrange 
            AppUser currentUser = _context.Users.FirstOrDefault();
            ReturnRequestController returnRequestController = new ReturnRequestController(_context, _mapper);
            returnRequestController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(currentUser.UserName), null)
                }
            };
            // Act 
            var result = await returnRequestController.Complete(12);
            var okobjectResult = (OkObjectResult)result;
            var resultValue = okobjectResult.Value as SuccessResponseResult<string>;

            // Assert
            Assert.True(resultValue.IsSuccessed);
            Assert.NotEmpty(resultValue.Result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Return request successfully!", resultValue.Result);
        }

        [Fact]
        public async Task ReturnRequest_FailAsync()
        {
            // Arrange 
            ReturnRequestController returnRequestController = new ReturnRequestController(_context, _mapper);

            // Act 
            var result = await returnRequestController.Complete(3000);
            var notFoundResult = (NotFoundObjectResult)result;
            var resultValue = notFoundResult.Value as ErrorResponseResult<string>;

            // Assert
            Assert.False(resultValue.IsSuccessed);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Return request does not exists!", resultValue.Message);
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
