using AssetManagement.Application.Controllers;
using AssetManagement.Contracts.Assignment.Response;
using AssetManagement.Contracts.Common;
using AssetManagement.Contracts.AutoMapper;
using AssetManagement.Data.EF;
using AssetManagement.Domain.Enums.Asset;
using AssetManagement.Domain.Models;
using AutoMapper;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;
using Newtonsoft.Json;
using static AssetManagement.Application.Tests.TestHelper.ConverterFromIActionResult;
using FluentAssertions;
using AssetManagement.Contracts.Assignment.Request;
using AssetManagement.Application.Tests.TestHelper;
using Microsoft.AspNetCore.Identity;

namespace AssetManagement.Application.Tests
{
    public class AssignmentControllerTests : IAsyncDisposable
    {
        private readonly DbContextOptions _options;
        private readonly AssetManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager; 

        public AssignmentControllerTests()
        {
            // Create InMemory dbcontext options
            _options = new DbContextOptionsBuilder<AssetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "AssignmentTestDb").Options;

            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new AssignmentProfile())).CreateMapper();

            // Create InMemory dbcontext with options
            _context = new AssetManagementDbContext(_options);
            _context.Database.EnsureDeleted();
            //SeedData();
            _context.Database.EnsureCreated();
        }

        #region GetAssignmentDetail
        [Fact]
        public async Task GetAssignmentDetail_Success_ReturnAssignmentDetail()
        {
            // Arrange 
            AssignmentsController assignmentController = new AssignmentsController(_context, _userManager, _mapper);

            // Act 
            var assignment = _mapper.Map<AssignmentDetailResponse>(await _context.Assignments
                .Include(x => x.Asset)
                .Include(x => x.AssignedToAppUser)
                .Include(x => x.AssignedByAppUser)
                .Where(a => a.Id == 1)
                .FirstOrDefaultAsync());
            string expected = JsonConvert.SerializeObject(assignment);
            var response = await assignmentController.GetAssignmentDetail(1);
            string result = ConvertOkObject<AssignmentDetailResponse>(response);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetAssignmentDetail_AssignmentNotExist_ReturnBadRequest()
        {
            // Arrange 
            AssignmentsController assignmentController = new AssignmentsController(_context, _userManager, _mapper);

            // Act 
            var result = await assignmentController.GetAssignmentDetail(0);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
        #endregion

        //private void SeedData()
        //{
        //    _context.Database.EnsureDeleted();
        //    //Create roles data
        //    List<AppRole> _roles = new()
        //    {
        //        new AppRole()
        //        {
        //            Id = new Guid("12147FE0-4571-4AD2-B8F7-D2C863EB78A5"),
        //            Name = "Admin",
        //            Description = "Admin role"
        //        },

        //        new AppRole()
        //        {
        //            Id = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC"),
        //            Name = "Staff",
        //            Description = "Staff role"
        //        }
        //    };
        //    //Create users data
        //    List<AppUser> _users = new()
        //    {
        //        new AppUser()
        //        {
        //            Id = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE"),
        //            FirstName = "Binh",
        //            LastName = "Nguyen Van",
        //            UserName = "binhnv",
        //            Email = "bnv@gmail.com",
        //            PasswordHash = "abc",
        //            Gender = Domain.Enums.AppUser.UserGender.Male,
        //            Location = Domain.Enums.AppUser.AppUserLocation.HoChiMinh,
        //            //RoleId = _roles[0].Id,
        //            IsLoginFirstTime = true,
        //            StaffCode = "SD01",
        //        },

        //        new AppUser()
        //        {
        //            Id = new Guid("70BD714F-9576-45BA-B5B7-F00649BE00DE"),
        //            FirstName = "An",
        //            LastName = "Nguyen Van",
        //            UserName = "annv",
        //            Email = "anv@gmail.com",
        //            PasswordHash = "xyz",
        //            Gender = Domain.Enums.AppUser.UserGender.Male,
        //            Location = Domain.Enums.AppUser.AppUserLocation.HaNoi,
        //            //RoleId = _roles[1].Id,
        //            IsLoginFirstTime = true,
        //            StaffCode = "SD02",
        //        }
        //    };
        //    //Add roles
        //    _context.AppRoles.AddRange(_roles);
        //    //Add users
        //    _context.AppUsers.AddRange(_users);
        //    _context.Assets.Add(new Asset
        //    {
        //        Id = 1,
        //        Name = $"Laptop 1",
        //        AssetCode = $"LT000001",
        //        Specification = $"This is laptop #1",
        //        InstalledDate = DateTime.Now.AddDays(-1),
        //        Category = null,
        //        Location = Domain.Enums.AppUser.AppUserLocation.HoChiMinh,
        //        State = State.Available,
        //        IsDeleted = false
        //    });
        //    _context.Assignments.Add(new Assignment
        //    {
        //        Id = 1,
        //        AssignedDate = DateTime.Now,
        //        ReturnedDate = DateTime.Now,
        //        State = Domain.Enums.Assignment.State.Accepted,
        //        AssetId = 1,
        //        AssignedTo = _users[0].Id,
        //        AssignedBy = _users[1].Id,
        //        Note = "Co len",
        //    });
        //    _context.SaveChanges();
        //}

        [Fact]
        public void GetAssignmentListByAssetCodeId_ReturnResults()
        {
            // Arrange 
            var assignmentController = new AssignmentsController(_context, _userManager, _mapper);

            // Act 
            var result = assignmentController.GetAssignmentsByAssetCodeId(1);

            var list = _context.Assignments.Where(x => x.AssetId == 1).ToList();

            var expected = _mapper.Map<List<AssignmentResponse>>(list);

            foreach (var item in expected)
            {
                item.AssignedTo = _context.Users.Find(new Guid(item.AssignedTo)).UserName;
                item.AssignedBy = _context.Users.Find(new Guid(item.AssignedBy)).UserName;
            }

            var okobjectResult = (OkObjectResult)result;
            var resultValue = (List<AssignmentResponse>)okobjectResult.Value;

            Assert.IsType<List<AssignmentResponse>>(resultValue);
            // Assert.NotEmpty(resultValue);
            Assert.Equal(resultValue.Count(), expected.Count());
        }

        [Fact]
        public void GetAssignmentListByAssetCodeId_ReturnEmptyResult()
        {
            // Arrange 
            var assignmentController = new AssignmentsController(_context, _userManager, _mapper);

            // Act 
            var result = assignmentController.GetAssignmentsByAssetCodeId(-1);

            var list = _context.Assignments.Where(x => x.AssetId == -1).ToList();

            var expected = _mapper.Map<List<AssignmentResponse>>(list);

            foreach (var item in expected)
            {
                item.AssignedTo = _context.Users.Find(new Guid(item.AssignedTo)).UserName;
                item.AssignedBy = _context.Users.Find(new Guid(item.AssignedBy)).UserName;
            }

            var okobjectResult = (OkObjectResult)result;
            var resultValue = (List<AssignmentResponse>)okobjectResult.Value;

            Assert.IsType<List<AssignmentResponse>>(resultValue);
            Assert.Empty(resultValue);
            Assert.Equal(resultValue.Count(), expected.Count());
        }

        #region GetList
        [Fact]
        public async Task GetList_ForDefault()
        {
            // Arrange 
            AssignmentsController assignmentController = new AssignmentsController(_context, _userManager, _mapper);

            // Act 
            var result = await assignmentController.Get(1, 2);

            var list = _context.Assignments
                .Where(x => !x.IsDeleted)
                .Select(x => new ViewListAssignmentResponse
                {
                    Id = x.Id,
                    AssetCode = x.Asset.AssetCode,
                    AssetName = x.Asset.Name,
                    AssignedTo = x.AssignedToAppUser.UserName,
                    AssignedBy = x.AssignedByAppUser.UserName,
                    AssignedDate = x.AssignedDate,
                    State = x.State,
                })
                .OrderBy(x => x.Id);

            var expected = StaticFunctions<ViewListAssignmentResponse>.Paging(list, 1, 2);

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewListPageResult<ViewListAssignmentResponse>)okobjectResult.Value;

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
            AssignmentsController assignmentController = new AssignmentsController(_context, _userManager, _mapper);
            var searchString = "SD";
            // Act 
            var result = await assignmentController.Get(1, 2, searchString);

            var list = _context.Assignments
                .Where(x => !x.IsDeleted && (x.Asset.Name.Contains(searchString) || x.Asset.AssetCode.Contains(searchString)))
                .Select(x => new ViewListAssignmentResponse
                {
                    Id = x.Id,
                    AssetCode = x.Asset.AssetCode,
                    AssetName = x.Asset.Name,
                    AssignedTo = x.AssignedToAppUser.UserName,
                    AssignedBy = x.AssignedByAppUser.UserName,
                    AssignedDate = x.AssignedDate,
                    State = x.State,
                })
                .OrderBy(x => x.Id);

            var expected = JsonConvert.SerializeObject(StaticFunctions<ViewListAssignmentResponse>.Paging(list, 1, 2));

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewListPageResult<ViewListAssignmentResponse>)okobjectResult.Value;

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
            AssignmentsController assignmentController = new AssignmentsController(_context, _userManager, _mapper);
            var searchString = "top 1";
            // Act 
            var result = await assignmentController.Get(1, 2, searchString);

            var list = _context.Assignments
                .Where(x => !x.IsDeleted && (x.Asset.Name.Contains(searchString) ||  x.Asset.AssetCode.Contains(searchString)))
                .Select(x => new ViewListAssignmentResponse
                {
                    Id = x.Id,
                    AssetCode = x.Asset.AssetCode,
                    AssetName = x.Asset.Name,
                    AssignedTo = x.AssignedToAppUser.UserName,
                    AssignedBy = x.AssignedByAppUser.UserName,
                    AssignedDate = x.AssignedDate,
                    State = x.State,
                })
                .OrderBy(x => x.Id);

            var expected = StaticFunctions<ViewListAssignmentResponse>.Paging(list, 1, 2);

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewListPageResult<ViewListAssignmentResponse>)okobjectResult.Value;

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
            AssignmentsController assignmentController = new AssignmentsController(_context, _userManager, _mapper);
            var state = (int)AssetManagement.Domain.Enums.Assignment.State.Accepted;
            // Act 
            var result = await assignmentController.Get(1, 2, stateFilter: state.ToString());

            var list = _context.Assignments
                .Where(x => !x.IsDeleted && (int)x.State == state)
                .Select(x => new ViewListAssignmentResponse
                {
                    Id = x.Id,
                    NoNumber = x.Id,
                    AssetCode = x.Asset.AssetCode,
                    AssetName = x.Asset.Name,
                    AssignedTo = x.AssignedToAppUser.UserName,
                    AssignedBy = x.AssignedByAppUser.UserName,
                    AssignedDate = x.AssignedDate,
                    State = x.State,
                })
                .OrderBy(x => x.Id);

            var expected = JsonConvert.SerializeObject(
                StaticFunctions<ViewListAssignmentResponse>.Paging(list, 1, 2));

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewListPageResult<ViewListAssignmentResponse>)okobjectResult.Value;

            var assignmentsList = JsonConvert.SerializeObject(resultValue.Data);

            Assert.Equal(expected, assignmentsList);
        }

        [Fact]
        public async Task GetList_FilterAssignedDate()
        {
            // Arrange 
            AssignmentsController assignmentController = new AssignmentsController(_context, _userManager, _mapper);
            var assignedDateFilter = "2022-11-28";
            // Act 
            var result = await assignmentController.Get(1, 2, assignedDateFilter);

            var list = _context.Assignments
                .Where(x => !x.IsDeleted && x.AssignedDate.Date == DateTime.Parse(assignedDateFilter).Date)
                .Select(x => new ViewListAssignmentResponse
                {
                    Id = x.Id,
                    NoNumber = x.Id,
                    AssetCode = x.Asset.AssetCode,
                    AssetName = x.Asset.Name,
                    AssignedTo = x.AssignedToAppUser.UserName,
                    AssignedBy = x.AssignedByAppUser.UserName,
                    AssignedDate = x.AssignedDate,
                    State = x.State,
                })
                .OrderBy(x => x.Id);
            var expected = JsonConvert.SerializeObject(
                StaticFunctions<ViewListAssignmentResponse>.Paging(list, 1, 2));

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewListPageResult<ViewListAssignmentResponse>)okobjectResult.Value;

            var assignmentsList = JsonConvert.SerializeObject(resultValue.Data);

            Assert.Equal(expected, assignmentsList);
        }

        [Fact]
        public async Task GetList_ForDefaultSortedByAssetCode()
        {
            // Arrange 
            AssignmentsController assignmentController = new AssignmentsController(_context, _userManager, _mapper);
            var sortType = "assetCode";
            // Act 
            var result = await assignmentController.Get(1, 2, sort: sortType);

            var list = _context.Assignments
                .Where(x => !x.IsDeleted)
                .Select(x => new ViewListAssignmentResponse
                {
                    Id = x.Id,
                    NoNumber = x.Id,
                    AssetCode = x.Asset.AssetCode,
                    AssetName = x.Asset.Name,
                    AssignedTo = x.AssignedToAppUser.UserName,
                    AssignedBy = x.AssignedByAppUser.UserName,
                    AssignedDate = x.AssignedDate,
                    State = x.State,
                }).OrderBy(x => x.AssetCode);

            var expected = JsonConvert.SerializeObject(
                StaticFunctions<ViewListAssignmentResponse>.Paging(list, 1, 2));

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewListPageResult<ViewListAssignmentResponse>)okobjectResult.Value;

            var assignmentsList = JsonConvert.SerializeObject(resultValue.Data);

            Assert.Equal(expected, assignmentsList);
        }

        [Fact]
        public async Task GetList_ForDefault_InvalidPaging()
        {
            // Arrange 
            AssignmentsController assignmentController = new AssignmentsController(_context, _userManager, _mapper);

            // Act 
            var result = await assignmentController.Get(-1, 2);

            var list = _context.Assignments
                .Where(x => !x.IsDeleted)
                .Select(x => new ViewListAssignmentResponse
                {
                    Id = x.Id,
                    AssetCode = x.Asset.AssetCode,
                    AssetName = x.Asset.Name,
                    AssignedTo = x.AssignedToAppUser.UserName,
                    AssignedBy = x.AssignedByAppUser.UserName,
                    AssignedDate = x.AssignedDate,
                    State = x.State,
                })
                .OrderBy(x => x.Id);

            var expected = StaticFunctions<ViewListAssignmentResponse>.Paging(list, -1, 2);

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewListPageResult<ViewListAssignmentResponse>)okobjectResult.Value;

            var assignmentsList = resultValue.Data;

            var isSorted = assignmentsList.SequenceEqual(expected);
            // Assert
            Assert.True(isSorted);
            Assert.Equal(assignmentsList.Count(), expected.Count());
        }
        #endregion

        #region UpdateAssignment
        [Fact]
        public async Task UpdateAssignment_Success_ReturnUpdatedAssignment()
        {
            // Arrange 
            AssignmentsController assignmentController = new AssignmentsController(_context, _userManager, _mapper);

            // Act 
            var updatingAssignment = await _context.Assignments
                .Include(a => a.Asset)
                .Include(a => a.AssignedToAppUser)
                .Where(a => a.Id == 9)
                .FirstOrDefaultAsync();
            DateTime updatedDate = DateTime.Now;

            var updateRequest = new UpdateAssignmentRequest
            {
                AssignToAppUserStaffCode = "SD0002",
                AssetCode = "LA100009",
                AssignedDate = updatedDate,
                Note = "haha"
            };

            var updatedAssignment = _mapper.Map<UpdateAssignmentResponse>(updatingAssignment);
            updatedAssignment.AssetId = 9;
            updatedAssignment.AssignedTo = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00BF");
            updatedAssignment.AssignedDate = updatedDate;
            updatedAssignment.Note = "haha";

            var response = await assignmentController.UpdateAssignment(9, updateRequest);
            string result = ConvertOkObject<UpdateAssignmentResponse>(response);
            string expected = JsonConvert.SerializeObject(updatedAssignment);

            //Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task UpdateAssignment_AssignmentNotFound_ReturnBadRequest()
        {
            // Arrange
            var assignmentController = new AssignmentsController(_context, _userManager, _mapper);

            // Act
            var result = await assignmentController.UpdateAssignment(0, new UpdateAssignmentRequest
            {
                AssignToAppUserStaffCode = "SD0002",
                AssetCode = "LA100009",
                AssignedDate = DateTime.Now,
                Note = "haha"
            });

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateAssignment_AssetNotFound_ReturnBadRequest()
        {
            // Arrange
            var assignmentController = new AssignmentsController(_context, _userManager, _mapper);

            // Act
            var result = await assignmentController.UpdateAssignment(9, new UpdateAssignmentRequest
            {
                AssignToAppUserStaffCode = "SD0002",
                AssetCode = "asdf",
                AssignedDate = DateTime.Now,
                Note = "haha"
            });

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateAssignment_UserNotFound_ReturnBadRequest()
        {
            // Arrange
            var assignmentController = new AssignmentsController(_context, _userManager, _mapper);

            // Act
            var result = await assignmentController.UpdateAssignment(9, new UpdateAssignmentRequest
            {
                AssignToAppUserStaffCode = "sdfa",
                AssetCode = "LA100009",
                AssignedDate = DateTime.Now,
                Note = "haha"
            });

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateAssignment_AcceptedAssignment_ReturnBadRequest()
        {
            // Arrange
            var assignmentController = new AssignmentsController(_context, _userManager, _mapper);

            // Act
            var result = await assignmentController.UpdateAssignment(2, new UpdateAssignmentRequest
            {
                AssignToAppUserStaffCode = "SD0002",
                AssetCode = "LA100009",
                AssignedDate = DateTime.Now,
                Note = "haha"
            });

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }


        //[Fact]
        //public async Task UpdateAssignment_NoChange_ReturnAssignment()
        //{
        //    // Arrange
        //    var assignmentController = new AssignmentsController(_context, _mapper);
        //    var updatingAssignment = await _context.Assignments
        //        .Include(a => a.Asset)
        //        .Include(a => a.AssignedToAppUser)
        //        .Where(a => a.Id == 9)
        //        .FirstOrDefaultAsync();
        //    var updatedAssignment = _mapper.Map<UpdateAssignmentResponse>(updatingAssignment);

        //    // Act
        //    var response = await assignmentController.UpdateAssignment(9, new UpdateAssignmentRequest
        //    {
        //        AssignToAppUserStaffCode = updatingAssignment.AssignedToAppUser.StaffCode,
        //        AssetCode = updatingAssignment.Asset.AssetCode,
        //        AssignedDate = updatingAssignment.AssignedDate,
        //        Note = updatingAssignment.Note
        //    });
        //    string expected = JsonConvert.SerializeObject(updatedAssignment);
        //    string result = ConvertOkObject<UpdateAssignmentResponse>(response);

        //    // Assert
        //    Assert.Equal(expected, result);
        //}

        #endregion

        #region DeleteAssignment
        #nullable disable
        #region DeleteSuccess
        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(7)]
        [InlineData(9)]
        public async Task Delete_SuccessAsync(int id)
        {
            //ARRANGE
            AssignmentsController controller = new(_context, _userManager, _mapper);

            //ACT
            IActionResult result = await controller.DeleteAsync(id);
            string data = ConverterFromIActionResult.ConvertOkObject<AssignmentResponse>(result);
            Assignment deleted = _context.Assignments.Find(id);
            AssignmentResponse expected = _mapper.Map<AssignmentResponse>(deleted);
            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.True(deleted.IsDeleted);
            Assert.Equal(JsonConvert.SerializeObject(expected), data);
        }
        #endregion

        #region Delete_Accepted
        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(6)]
        public async Task Delete_AcceptedAsync(int id)
        {
            //ARRANGE
            AssignmentsController controller = new(_context, _userManager, _mapper);

            //ACT
            IActionResult result = await controller.DeleteAsync(id);
            string data = ConvertStatusCode(result);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("\"Assignment is Accepted and cannot be deleted\"", data);
        }
        #endregion

        #region Delete_NotFound
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(11)]
        [InlineData(999)]
        public async Task Delete_NotFoundAsync(int id)
        {
            //ARRANGE
            AssignmentsController controller = new(_context, _userManager, _mapper);

            //ACT
            IActionResult result = await controller.DeleteAsync(id);
            string data = ConverterFromIActionResult.ConvertStatusCode(result);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            
            Assert.Equal("\"Assignment does not exist\"", data);
        }
        #endregion

        #region DeleteException
        [Fact]
        public async Task Delete_ExceptionAsync()
        {
            //ARRANGE
            //Use null mapper to cause exception
            AssignmentsController controller = new(_context, _userManager, null);

            //ACT
            IActionResult result = await controller.DeleteAsync(1);
            string data = ConverterFromIActionResult.ConvertStatusCode(result);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("\"Object reference not set to an instance of an object.\"", data);
        }
        #endregion
        #endregion

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }
    }
}
