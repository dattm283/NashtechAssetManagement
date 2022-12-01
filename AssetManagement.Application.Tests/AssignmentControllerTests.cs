using AssetManagement.Application.Controllers;
using AssetManagement.Contracts.Asset.Response;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;
using static AssetManagement.Application.Tests.TestHelper.ConverterFromIActionResult;
using FluentAssertions;

namespace AssetManagement.Application.Tests
{
    public class AssignmentControllerTests : IDisposable
    {
        private readonly DbContextOptions _options;
        private readonly AssetManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AssignmentControllerTests()
        {
            // Create InMemory dbcontext options
            _options = new DbContextOptionsBuilder<AssetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "AssetTestDb1").Options;

            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new UserProfile())).CreateMapper();

            // Create InMemory dbcontext with options
            _context = new AssetManagementDbContext(_options);
            //SeedData();
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        #region GetAssignmentDetail
        [Fact]
        public async Task GetAssignmentDetail_Success_ReturnAssignmentDetail()
        {
            // Arrange 
            AssignmentsController assignmentController = new AssignmentsController(null, _context, _mapper);

            // Act 
            var assignment = _mapper.Map<AssignmentDetailResponse>(await _context.Assignments
                .Include(x => x.Asset)
                .Include(x => x.AssignedToAppUser)
                .Include(x => x.AssignedByToAppUser)
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
            AssignmentsController assignmentController = new AssignmentsController(null, _context, _mapper);

            // Act 
            var result = await assignmentController.GetAssignmentDetail(0);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
        #endregion

        private void SeedData()
        {
            _context.Database.EnsureDeleted();
            //Create roles data
            List<AppRole> _roles = new()
            {
                new AppRole()
                {
                    Id = new Guid("12147FE0-4571-4AD2-B8F7-D2C863EB78A5"),
                    Name = "Admin",
                    Description = "Admin role"
                },

                new AppRole()
                {
                    Id = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC"),
                    Name = "Staff",
                    Description = "Staff role"
                }
            };
            //Create users data
            List<AppUser> _users = new()
            {
                new AppUser()
                {
                    Id = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE"),
                    FirstName = "Binh",
                    LastName = "Nguyen Van",
                    UserName = "binhnv",
                    Email = "bnv@gmail.com",
                    PasswordHash = "abc",
                    Gender = Domain.Enums.AppUser.UserGender.Male,
                    Location = Domain.Enums.AppUser.AppUserLocation.HoChiMinh,
                    //RoleId = _roles[0].Id,
                    IsLoginFirstTime = true,
                    StaffCode = "SD01",
                },

                new AppUser()
                {
                    Id = new Guid("70BD714F-9576-45BA-B5B7-F00649BE00DE"),
                    FirstName = "An",
                    LastName = "Nguyen Van",
                    UserName = "annv",
                    Email = "anv@gmail.com",
                    PasswordHash = "xyz",
                    Gender = Domain.Enums.AppUser.UserGender.Male,
                    Location = Domain.Enums.AppUser.AppUserLocation.HaNoi,
                    //RoleId = _roles[1].Id,
                    IsLoginFirstTime = true,
                    StaffCode = "SD02",
                }
            };
            //Add roles
            _context.AppRoles.AddRange(_roles);
            //Add users
            _context.AppUsers.AddRange(_users);
            _context.Assets.Add(new Asset
            {
                Id = 1,
                Name = $"Laptop 1",
                AssetCode = $"LT000001",
                Specification = $"This is laptop #1",
                InstalledDate = DateTime.Now.AddDays(-1),
                Category = null,
                Location = Domain.Enums.AppUser.AppUserLocation.HoChiMinh,
                State = State.Available,
                IsDeleted = false
            });
            _context.Assignments.Add(new Assignment
            {
                Id = 1,
                AssignedDate = DateTime.Now,
                ReturnedDate = DateTime.Now,
                State = Domain.Enums.Assignment.State.Accepted,
                AssetId = 1,
                AssignedTo = _users[0].Id,
                AssignedBy = _users[1].Id,
                Note = "Co len",
            });
            _context.SaveChanges();
        }

        // [Fact]
        // public void GetAssignmentListByAssetCodeId_ReturnResults()
        // {
        //     // Arrange 
        //     var assignmentController = new AssignmentController(_context, _mapper);

        //     // Act 
        //     var result = assignmentController.GetAssignmentsByAssetCodeId(1);

        //     var list = _context.Assignments.Where(x => x.AssetId == 1).ToList();

        //     var expected = _mapper.Map<List<AssignmentResponse>>(list);

        //     foreach (var item in expected)
        //     {
        //         item.AssignedTo = _context.Users.Find(new Guid(item.AssignedTo)).UserName;
        //         item.AssignedBy = _context.Users.Find(new Guid(item.AssignedBy)).UserName;
        //     }

        //     var okobjectResult = (OkObjectResult)result;
        //     var resultValue = (List<AssignmentResponse>)okobjectResult.Value;

        //     Assert.IsType<List<AssignmentResponse>>(resultValue);
        //     Assert.NotEmpty(resultValue);
        //     Assert.Equal(resultValue.Count(), expected.Count());
        // }

        // [Fact]
        // public void GetAssignmentListByAssetCodeId_ReturnEmptyResult()
        // {
        //     // Arrange 
        //     var assignmentController = new AssignmentController(_context, _mapper);

        //     // Act 
        //     var result = assignmentController.GetAssignmentsByAssetCodeId(2);

        //     var list = _context.Assignments.Where(x => x.AssetId == 2).ToList();

        //     var expected = _mapper.Map<List<AssignmentResponse>>(list);

        //     foreach (var item in expected)
        //     {
        //         item.AssignedTo = _context.Users.Find(new Guid(item.AssignedTo)).UserName;
        //         item.AssignedBy = _context.Users.Find(new Guid(item.AssignedBy)).UserName;
        //     }

        //     var okobjectResult = (OkObjectResult)result;
        //     var resultValue = (List<AssignmentResponse>)okobjectResult.Value;

        //     Assert.IsType<List<AssignmentResponse>>(resultValue);
        //     Assert.Empty(resultValue);
        //     Assert.Equal(resultValue.Count(), expected.Count());
        // }

        #region GetList
        [Fact]
        public async Task GetList_ForDefault()
        {
            // Arrange 
            AssignmentsController assignmentController = new AssignmentsController(_context, _mapper);

            // Act 
            var result = await assignmentController.Get(1, 2);

            // var listDefault = _context.Assignments
            //     .Include(x => x.Asset)
            //     .Include(x => x.AssignedToAppUser)
            //     .Include(x => x.AssignedByToAppUser)
            //     .Where(x => !x.IsDeleted)
            //     .Select(x => new ViewListAssignmentResponse
            //     {
            //         Id = x.Id,
            //         AssetCode = x.Asset.AssetCode,
            //         AssetName = x.Asset.Name,
            //         AssignedTo = x.AssignedToAppUser.UserName,
            //         AssignedBy = x.AssignedByToAppUser.UserName,
            //         AssignedDate = x.AssignedDate,
            //         State = x.State,
            //     })
            //     .OrderBy(x => x.Id)
            //     .ToList();

            // var list = listDefault.Select((x, index) => new ViewListAssignmentResponse
            // {
            //     Id = x.Id,
            //     NoNumber = index + 1,
            //     AssetCode = x.AssetCode,
            //     AssetName = x.AssetName,
            //     AssignedTo = x.AssignedTo,
            //     AssignedBy = x.AssignedBy,
            //     AssignedDate = x.AssignedDate,
            //     State = x.State,
            // }).AsQueryable<ViewListAssignmentResponse>();

            var list = _context.Assignments
                .Where(x => !x.IsDeleted)
                .Select(x => new ViewListAssignmentResponse
                {
                    Id = x.Id,
                    AssetCode = x.Asset.AssetCode,
                    AssetName = x.Asset.Name,
                    AssignedTo = x.AssignedToAppUser.UserName,
                    AssignedBy = x.AssignedByToAppUser.UserName,
                    AssignedDate = x.AssignedDate,
                    State = x.State,
                })
                .OrderBy(x => x.Id);

            var expected = StaticFunctions<ViewListAssignmentResponse>.Paging(list, 1, 2);

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewList_ListResponse<ViewListAssignmentResponse>)okobjectResult.Value;

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
            AssignmentsController assignmentController = new AssignmentsController(_context, _mapper);
            var searchString = "top 1";
            // Act 
            var result = await assignmentController.Get(1, 2, searchString);

            // var listDefault = _context.Assignments
            //     .Include(x => x.Asset)
            //     .Include(x => x.AssignedToAppUser)
            //     .Include(x => x.AssignedByToAppUser)
            //     .Where(x => !x.IsDeleted)
            //     .Select(x => new ViewListAssignmentResponse
            //     {
            //         Id = x.Id,
            //         AssetCode = x.Asset.AssetCode,
            //         AssetName = x.Asset.Name,
            //         AssignedTo = x.AssignedToAppUser.UserName,
            //         AssignedBy = x.AssignedByToAppUser.UserName,
            //         AssignedDate = x.AssignedDate,
            //         State = x.State,
            //     })
            //     .OrderBy(x => x.Id)
            //     .ToList();

            // var list = listDefault
            // .Where(x => (x.AssetName.Contains(searchString) || x.AssetCode.Contains(searchString)))
            // .Select((x, index) => new ViewListAssignmentResponse
            // {
            //     Id = x.Id,
            //     NoNumber = index + 1,
            //     AssetCode = x.AssetCode,
            //     AssetName = x.AssetName,
            //     AssignedTo = x.AssignedTo,
            //     AssignedBy = x.AssignedBy,
            //     AssignedDate = x.AssignedDate,
            //     State = x.State,
            // }).AsQueryable<ViewListAssignmentResponse>();

            var list = _context.Assignments
                .Where(x => !x.IsDeleted && (x.Asset.Name.Contains(searchString) || x.Asset.AssetCode.Contains(searchString)))
                .Select(x => new ViewListAssignmentResponse
                {
                    Id = x.Id,
                    AssetCode = x.Asset.AssetCode,
                    AssetName = x.Asset.Name,
                    AssignedTo = x.AssignedToAppUser.UserName,
                    AssignedBy = x.AssignedByToAppUser.UserName,
                    AssignedDate = x.AssignedDate,
                    State = x.State,
                })
                .OrderBy(x => x.Id);

            var expected = JsonConvert.SerializeObject(StaticFunctions<ViewListAssignmentResponse>.Paging(list, 1, 2));

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewList_ListResponse<ViewListAssignmentResponse>)okobjectResult.Value;

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
            AssignmentsController assignmentController = new AssignmentsController(_context, _mapper);
            var searchString = "top 1";
            // Act 
            var result = await assignmentController.Get(1, 2, searchString);

            // var listDefault = _context.Assignments
            //     .Include(x => x.Asset)
            //     .Include(x => x.AssignedToAppUser)
            //     .Include(x => x.AssignedByToAppUser)
            //     .Where(x => !x.IsDeleted)
            //     .Select(x => new ViewListAssignmentResponse
            //     {
            //         Id = x.Id,
            //         AssetCode = x.Asset.AssetCode,
            //         AssetName = x.Asset.Name,
            //         AssignedTo = x.AssignedToAppUser.UserName,
            //         AssignedBy = x.AssignedByToAppUser.UserName,
            //         AssignedDate = x.AssignedDate,
            //         State = x.State,
            //     })
            //     .OrderBy(x => x.Id)
            //     .ToList();

            // var list = listDefault
            // .Where(x => (x.AssetName.Contains(searchString) || x.AssetCode.Contains(searchString)))
            // .Select((x, index) => new ViewListAssignmentResponse
            // {
            //     Id = x.Id,
            //     NoNumber = index + 1,
            //     AssetCode = x.AssetCode,
            //     AssetName = x.AssetName,
            //     AssignedTo = x.AssignedTo,
            //     AssignedBy = x.AssignedBy,
            //     AssignedDate = x.AssignedDate,
            //     State = x.State,
            // }).AsQueryable<ViewListAssignmentResponse>();

            var list = _context.Assignments
                .Where(x => !x.IsDeleted && (x.Asset.Name.Contains(searchString) ||  x.Asset.AssetCode.Contains(searchString)))
                .Select(x => new ViewListAssignmentResponse
                {
                    Id = x.Id,
                    AssetCode = x.Asset.AssetCode,
                    AssetName = x.Asset.Name,
                    AssignedTo = x.AssignedToAppUser.UserName,
                    AssignedBy = x.AssignedByToAppUser.UserName,
                    AssignedDate = x.AssignedDate,
                    State = x.State,
                })
                .OrderBy(x => x.Id);

            var expected = StaticFunctions<ViewListAssignmentResponse>.Paging(list, 1, 2);

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewList_ListResponse<ViewListAssignmentResponse>)okobjectResult.Value;

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
            AssignmentsController assignmentController = new AssignmentsController(_context, _mapper);
            var state = (int)AssetManagement.Domain.Enums.Assignment.State.Accepted;
            // Act 
            var result = await assignmentController.Get(1, 2, state.ToString());

            // var listDefault = _context.Assignments
            //     .Include(x => x.Asset)
            //     .Include(x => x.AssignedToAppUser)
            //     .Include(x => x.AssignedByToAppUser)
            //     .Where(x => !x.IsDeleted)
            //     .Select(x => new ViewListAssignmentResponse
            //     {
            //         Id = x.Id,
            //         AssetCode = x.Asset.AssetCode,
            //         AssetName = x.Asset.Name,
            //         AssignedTo = x.AssignedToAppUser.UserName,
            //         AssignedBy = x.AssignedByToAppUser.UserName,
            //         AssignedDate = x.AssignedDate,
            //         State = x.State,
            //     })
            //     .OrderBy(x => x.Id)
            //     .ToList();

            // var list = listDefault
            // .Where(x => (int)x.State == state)
            // .Select((x, index) => new ViewListAssignmentResponse
            // {
            //     Id = x.Id,
            //     NoNumber = index + 1,
            //     AssetCode = x.AssetCode,
            //     AssetName = x.AssetName,
            //     AssignedTo = x.AssignedTo,
            //     AssignedBy = x.AssignedBy,
            //     AssignedDate = x.AssignedDate,
            //     State = x.State,
            // }).AsQueryable<ViewListAssignmentResponse>();

            var list = _context.Assignments
                .Where(x => !x.IsDeleted && (int)x.State == state)
                .Select(x => new ViewListAssignmentResponse
                {
                    Id = x.Id,
                    AssetCode = x.Asset.AssetCode,
                    AssetName = x.Asset.Name,
                    AssignedTo = x.AssignedToAppUser.UserName,
                    AssignedBy = x.AssignedByToAppUser.UserName,
                    AssignedDate = x.AssignedDate,
                    State = x.State,
                })
                .OrderBy(x => x.Id);

            var expected = StaticFunctions<ViewListAssignmentResponse>.Paging(list, 1, 2);

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewList_ListResponse<ViewListAssignmentResponse>)okobjectResult.Value;

            var assignmentsList = resultValue.Data;

            var isSorted = assignmentsList.SequenceEqual(expected);
            // Assert
            Assert.True(isSorted);
            Assert.Equal(assignmentsList.Count(), expected.Count());
        }

        [Fact]
        public async Task GetList_FilterAssignedDate()
        {
            // Arrange 
            AssignmentsController assignmentController = new AssignmentsController(_context, _mapper);
            var assignedDateFilter = "2022-11-28";
            // Act 
            var result = await assignmentController.Get(1, 2, assignedDateFilter);

            // var listDefault = _context.Assignments
            //     .Include(x => x.Asset)
            //     .Include(x => x.AssignedToAppUser)
            //     .Include(x => x.AssignedByToAppUser)
            //     .Where(x => !x.IsDeleted)
            //     .Select(x => new ViewListAssignmentResponse
            //     {
            //         Id = x.Id,
            //         AssetCode = x.Asset.AssetCode,
            //         AssetName = x.Asset.Name,
            //         AssignedTo = x.AssignedToAppUser.UserName,
            //         AssignedBy = x.AssignedByToAppUser.UserName,
            //         AssignedDate = x.AssignedDate,
            //         State = x.State,
            //     })
            //     .OrderBy(x => x.Id)
            //     .ToList();

            // var list = listDefault
            // .Where(x => x.AssignedDate.Date == DateTime.Parse(assignedDateFilter).Date)
            // .Select((x, index) => new ViewListAssignmentResponse
            // {
            //     Id = x.Id,
            //     NoNumber = index + 1,
            //     AssetCode = x.AssetCode,
            //     AssetName = x.AssetName,
            //     AssignedTo = x.AssignedTo,
            //     AssignedBy = x.AssignedBy,
            //     AssignedDate = x.AssignedDate,
            //     State = x.State,
            // }).AsQueryable<ViewListAssignmentResponse>();

            var list = _context.Assignments
                .Where(x => !x.IsDeleted && x.AssignedDate.Date == DateTime.Parse(assignedDateFilter).Date)
                .Select(x => new ViewListAssignmentResponse
                {
                    Id = x.Id,
                    AssetCode = x.Asset.AssetCode,
                    AssetName = x.Asset.Name,
                    AssignedTo = x.AssignedToAppUser.UserName,
                    AssignedBy = x.AssignedByToAppUser.UserName,
                    AssignedDate = x.AssignedDate,
                    State = x.State,
                })
                .OrderBy(x => x.Id);
            var expected = StaticFunctions<ViewListAssignmentResponse>.Paging(list, 1, 2);

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewList_ListResponse<ViewListAssignmentResponse>)okobjectResult.Value;

            var assignmentsList = resultValue.Data;

            var isSorted = assignmentsList.SequenceEqual(expected);
            // Assert
            Assert.True(isSorted);
            Assert.Equal(assignmentsList.Count(), expected.Count());
        }

        [Fact]
        public async Task GetList_ForDefaultSortedByAssetCode()
        {
            // Arrange 
            AssignmentsController assignmentController = new AssignmentsController(_context, _mapper);
            var sortType = "assetCode";
            // Act 
            var result = await assignmentController.Get(1, 2, sortType);

            // var listDefault = _context.Assignments
            //     .Include(x => x.Asset)
            //     .Include(x => x.AssignedToAppUser)
            //     .Include(x => x.AssignedByToAppUser)
            //     .Where(x => !x.IsDeleted)
            //     .Select(x => new ViewListAssignmentResponse
            //     {
            //         Id = x.Id,
            //         AssetCode = x.Asset.AssetCode,
            //         AssetName = x.Asset.Name,
            //         AssignedTo = x.AssignedToAppUser.UserName,
            //         AssignedBy = x.AssignedByToAppUser.UserName,
            //         AssignedDate = x.AssignedDate,
            //         State = x.State,
            //     })
            //     .ToList();

            // var list = listDefault
            // .Select((x, index) => new ViewListAssignmentResponse
            // {
            //     Id = x.Id,
            //     NoNumber = index + 1,
            //     AssetCode = x.AssetCode,
            //     AssetName = x.AssetName,
            //     AssignedTo = x.AssignedTo,
            //     AssignedBy = x.AssignedBy,
            //     AssignedDate = x.AssignedDate,
            //     State = x.State,
            // }).OrderBy(x => x.AssetCode)
            // .AsQueryable<ViewListAssignmentResponse>();

            var list = _context.Assignments
                .Where(x => !x.IsDeleted)
                .Select(x => new ViewListAssignmentResponse
                {
                    Id = x.Id,
                    AssetCode = x.Asset.AssetCode,
                    AssetName = x.Asset.Name,
                    AssignedTo = x.AssignedToAppUser.UserName,
                    AssignedBy = x.AssignedByToAppUser.UserName,
                    AssignedDate = x.AssignedDate,
                    State = x.State,
                }).OrderBy(x => x.AssetCode);

            var expected = StaticFunctions<ViewListAssignmentResponse>.Paging(list, 1, 2);

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewList_ListResponse<ViewListAssignmentResponse>)okobjectResult.Value;

            var assignmentsList = resultValue.Data;

            var isSorted = assignmentsList.SequenceEqual(expected);
            // Assert
            Assert.True(isSorted);
            Assert.Equal(assignmentsList.Count(), expected.Count());
        }

        [Fact]
        public async Task GetList_ForDefault_InvalidPaging()
        {
            // Arrange 
            AssignmentsController assignmentController = new AssignmentsController(_context, _mapper);

            // Act 
            var result = await assignmentController.Get(-1, 2);

            // var listDefault = _context.Assignments
            //     .Include(x => x.Asset)
            //     .Include(x => x.AssignedToAppUser)
            //     .Include(x => x.AssignedByToAppUser)
            //     .Where(x => !x.IsDeleted)
            //     .Select(x => new ViewListAssignmentResponse
            //     {
            //         Id = x.Id,
            //         AssetCode = x.Asset.AssetCode,
            //         AssetName = x.Asset.Name,
            //         AssignedTo = x.AssignedToAppUser.UserName,
            //         AssignedBy = x.AssignedByToAppUser.UserName,
            //         AssignedDate = x.AssignedDate,
            //         State = x.State,
            //     })
            //     .OrderBy(x => x.Id)
            //     .ToList();

            // var list = listDefault.Select((x, index) => new ViewListAssignmentResponse
            // {
            //     Id = x.Id,
            //     NoNumber = index + 1,
            //     AssetCode = x.AssetCode,
            //     AssetName = x.AssetName,
            //     AssignedTo = x.AssignedTo,
            //     AssignedBy = x.AssignedBy,
            //     AssignedDate = x.AssignedDate,
            //     State = x.State,
            // }).AsQueryable<ViewListAssignmentResponse>();

            var list = _context.Assignments
                .Where(x => !x.IsDeleted)
                .Select(x => new ViewListAssignmentResponse
                {
                    Id = x.Id,
                    AssetCode = x.Asset.AssetCode,
                    AssetName = x.Asset.Name,
                    AssignedTo = x.AssignedToAppUser.UserName,
                    AssignedBy = x.AssignedByToAppUser.UserName,
                    AssignedDate = x.AssignedDate,
                    State = x.State,
                })
                .OrderBy(x => x.Id);

            var expected = StaticFunctions<ViewListAssignmentResponse>.Paging(list, -1, 2);

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewList_ListResponse<ViewListAssignmentResponse>)okobjectResult.Value;

            var assignmentsList = resultValue.Data;

            var isSorted = assignmentsList.SequenceEqual(expected);
            // Assert
            Assert.True(isSorted);
            Assert.Equal(assignmentsList.Count(), expected.Count());
        }
        #endregion

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
