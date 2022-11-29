using AssetManagement.Application.Controllers;
using AssetManagement.Contracts.Asset.Response;
using AssetManagement.Contracts.Assignment.Response;
using AssetManagement.Contracts.AutoMapper;
using AssetManagement.Data.EF;
using AssetManagement.Domain.Enums.Asset;
using AssetManagement.Domain.Models;
using AutoMapper;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AssetManagement.Application.Tests
{
    public class AssignmentControllerTests: IDisposable
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

            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new AssignmentProfile())).CreateMapper();

            // Create InMemory dbcontext with options
            _context = new AssetManagementDbContext(_options);
            _context.Database.EnsureDeleted();
            SeedData();
        }

        private void SeedData()
        {
            _context.Database.EnsureDeleted();
            //Create roles data
            List<AppRole> _roles = new ()
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
                    Id= new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE"),
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
                Note="Co len",
            });
            _context.SaveChanges();
        }

        [Fact]
        public void GetAssignmentListByAssetCodeId_ReturnResults()
        {
            // Arrange 
            var assignmentController = new AssignmentController(_context, _mapper);

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
            Assert.NotEmpty(resultValue);
            Assert.Equal(resultValue.Count(), expected.Count());
        }

        [Fact]
        public void GetAssignmentListByAssetCodeId_ReturnEmptyResult()
        {
            // Arrange 
            var assignmentController = new AssignmentController(_context, _mapper);

            // Act 
            var result = assignmentController.GetAssignmentsByAssetCodeId(2);

            var list = _context.Assignments.Where(x => x.AssetId == 2).ToList();

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

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
