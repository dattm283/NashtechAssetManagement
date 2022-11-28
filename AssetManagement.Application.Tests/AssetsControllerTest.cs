﻿using AssetManagement.Application.Controllers;
using AssetManagement.Contracts.Asset.Request;
using AssetManagement.Contracts.Asset.Response;
using AssetManagement.Contracts.AutoMapper;
using AssetManagement.Data.EF;
using AssetManagement.Domain.Enums.Asset;
using AssetManagement.Domain.Models;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AssetManagement.Application.Tests.TestHelper.ConverterFromIActionResult;
using Xunit;
using AssetManagement.Contracts.Asset.Request;
using AssetManagement.Application.Tests.TestHelper;
using AssetManagement.Contracts.Common;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;

#nullable disable
namespace AssetManagement.Application.Tests
{
    public class AssetsControllerTest: IDisposable
    {
        private readonly DbContextOptions _options;
        private readonly AssetManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private List<Asset> _assets;
        private List<Category> _categories;

        public AssetsControllerTest()
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

        #region CreateAsset
        [Fact]
        public async Task CreateAsset_SuccessAsync()
        {
            //ARRANGE
            CreateAssetRequest request = new()
            {
                CategoryId = 2,
                Name = "Laptop 21",
                Specification = "This is laptop #21",
                InstalledDate = DateTime.Now,
                State = (int)(State.Available)
            };

            AssetsController controller = new(_context, _mapper);
            AppUser user = _context.Users.FirstOrDefault();
            //Create context for controller with fake login
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(user.UserName), null)
                }
            };

            //ACT
            var response = await controller.CreateAssetAsync(request);
            string result = ConvertOkObject<SuccessResponseResult<string>>(response);
            Asset newAsset = _context.Assets.LastOrDefault();
            var expected = JsonConvert.SerializeObject(
                new SuccessResponseResult<string>("Create Asset sucessfully"));

            //ASSERT
            Assert.NotNull(response);
            Assert.Equal(expected, result);
            Assert.Equal(newAsset.Name, request.Name);
            Assert.Equal("MO000001", newAsset.AssetCode);
            Assert.Equal(user.Location, newAsset.Location);
        }

        [Fact]
        public async Task CreateAsset_BadRequest_InvalidCategoryAsync()
        {
            //ARRANGE
            CreateAssetRequest request = new()
            {
                CategoryId = -1,
                Name = "Laptop 21",
                Specification = "This is laptop #21",
                InstalledDate = DateTime.Now,
                State = (int)State.Available
            };

            AssetsController controller = new(_context, _mapper);
            AppUser user = _context.Users.FirstOrDefault();
            //Create context for controller with fake login
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new GenericPrincipal(new GenericIdentity(user.UserName), null)
                }
            };

            //ACT
            IActionResult response = await controller.CreateAssetAsync(request);
            var result = (response as ObjectResult).Value;

            //ASSERT
            Assert.NotNull(response);
            Assert.IsType<ErrorResponseResult<string>>(result);
            Assert.False(((ErrorResponseResult<string>)result).IsSuccessed);
            Assert.Equal("Invalid Category", ((ErrorResponseResult<string>)result).Message);
        }
        #endregion

        #region DeleteAsset
        [Fact]
        public async Task DeleteAsset_Success_ReturnDeletedAsset()
        {
            // Arrange 
            AssetsController assetController = new AssetsController(_context, _mapper);
            var deletedAsset = _mapper
                .Map<DeleteAssetReponse>(await _context.Assets
                    .Where(a => a.Id == 1)
                    .FirstOrDefaultAsync());
            deletedAsset.IsDeleted = true;

            // Act 
            var result = await assetController.DeleteAsset(1);

            string resultObject = ConvertOkObject<DeleteAssetReponse>(result);
            string expectedObject = JsonConvert.SerializeObject(deletedAsset);

            // Assert
            Assert.Equal(resultObject, expectedObject);
        }

        [Fact]
        public async Task DeleteAsset_Invalid_ReturnBadRequest()
        {
            // Arrange 
            AssetsController assetController = new AssetsController(_context, _mapper);

            // Act 
            var result = await assetController.DeleteAsset(0);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();

        }
        #endregion

        #region GetList
        [Fact]
        public async Task GetList_ForDefault()
        {
            // Arrange 
            AssetsController assetController = new AssetsController(_context, _mapper);

            // Act 
            var result = await assetController.Get(1, 2);

            var query = _context.Assets
                .Include(x => x.Category)
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Name);

            var list = StaticFunctions<Asset>.Paging(query, 1, 2);

            var expected = _mapper.Map<List<ViewListAssets_AssetResponse>>(list);

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewList_ListResponse<ViewListAssets_AssetResponse>)okobjectResult.Value;

            var assetsList = resultValue.Data;

            var isSorted = assetsList.SequenceEqual(expected);
            // Assert
            Assert.True(isSorted);
            Assert.Equal(assetsList.Count(), expected.Count());
        }

        [Fact]
        public async Task GetList_SearchString_WithData()
        {
            // Arrange 
            AssetsController assetController = new AssetsController(_context, _mapper);

            var searchString = "top 1";

            // Act 
            var result = await assetController.Get(1, 2, searchString);

            var query = _context.Assets.Include(x => x.Category)
                .Where(x => (x.Name.Contains(searchString) || x.AssetCode.Contains(searchString))
                    && !x.IsDeleted)
                .OrderBy(x => x.Name);

            var list = StaticFunctions<Asset>.Paging(query, 1, 2);

            var expected = JsonConvert.SerializeObject(_mapper.Map<List<ViewListAssets_AssetResponse>>(list));

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewList_ListResponse<ViewListAssets_AssetResponse>)okobjectResult.Value;

            var assetsList = JsonConvert.SerializeObject(resultValue.Data);

            var isSorted = assetsList.SequenceEqual(expected);
            Assert.True(isSorted);
            //Assert.Equal(expected, assetsList);
            Assert.Equal(assetsList.Count(), expected.Count());
        }

        [Fact]
        public async Task GetList_SearchString_WithOutData()
        {
            // Arrange 
            AssetsController assetController = new AssetsController(_context, _mapper);

            var searchString = "Nash 1";

            // Act 
            var result = await assetController.Get(1, 2, searchString);

            var query = _context.Assets
                .Include(x => x.Category)
                .Where(x => (x.Name.Contains(searchString) || x.AssetCode.Contains(searchString))
                    && !x.IsDeleted)
                .OrderBy(x => x.Name);

            var list = StaticFunctions<Asset>.Paging(query, 1, 2);

            var expected = _mapper.Map<List<ViewListAssets_AssetResponse>>(list);

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewList_ListResponse<ViewListAssets_AssetResponse>)okobjectResult.Value;

            var assetsList = resultValue.Data;

            var isSorted = assetsList.SequenceEqual(expected);
            // Assert
            Assert.True(isSorted);
            Assert.Equal(assetsList.Count(), expected.Count());
        }

        [Fact]
        public async Task GetList_FilterState()
        {
            // Arrange 
            AssetsController assetController = new AssetsController(_context, _mapper);
            var state = (int)AssetManagement.Domain.Enums.Asset.State.Available;
            // Act 
            var result = await assetController.Get(1, 2, "", "", state.ToString());

            var query = _context.Assets
                .Include(x => x.Category)
                .Where(x => (int)x.State == state && !x.IsDeleted)
                .OrderBy(x => x.Name);

            var list = StaticFunctions<Asset>.Paging(query, 1, 2);

            var expected = _mapper.Map<List<ViewListAssets_AssetResponse>>(list);

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewList_ListResponse<ViewListAssets_AssetResponse>)okobjectResult.Value;

            var assetsList = resultValue.Data;

            var isSorted = assetsList.SequenceEqual(expected);
            // Assert
            Assert.True(isSorted);
            Assert.Equal(assetsList.Count(), expected.Count());
        }

        [Fact]
        public async Task GetList_ForDefaultSorted()
        {
            // Arrange 
            AssetsController assetController = new AssetsController(_context, _mapper);

            var sortType = "id";
            // Act 
            var result = await assetController.Get(1, 2, "", "", "", sortType);

            var query = _context.Assets
                .Include(x => x.Category)
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Id);

            var list = StaticFunctions<Asset>.Paging(query, 1, 2);

            var expected = _mapper.Map<List<ViewListAssets_AssetResponse>>(list);

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewList_ListResponse<ViewListAssets_AssetResponse>)okobjectResult.Value;

            var assetsList = resultValue.Data;

            var isSorted = assetsList.SequenceEqual(expected);
            // Assert
            Assert.Equal(assetsList.Count(), expected.Count());
            Assert.True(isSorted);
        }

        [Fact]
        public async Task GetList_ForDefaultSortedByCode()
        {
            // Arrange 
            AssetsController assetController = new AssetsController(_context, _mapper);

            var sortType = "assetCode";
            // Act 
            var result = await assetController.Get(1, 2, "", "", "", sortType);

            var query = _context.Assets
                .Include(x => x.Category)
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.AssetCode);

            var list = StaticFunctions<Asset>.Paging(query, 1, 2);

            var expected = _mapper.Map<List<ViewListAssets_AssetResponse>>(list);

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewList_ListResponse<ViewListAssets_AssetResponse>)okobjectResult.Value;

            var assetsList = resultValue.Data;

            var isSorted = assetsList.SequenceEqual(expected);
            // Assert
            Assert.Equal(assetsList.Count(), expected.Count());
            Assert.True(isSorted);
        }

        [Fact]
        public async Task GetList_ForDefaultSortedByState()
        {
            // Arrange 
            AssetsController assetController = new AssetsController(_context, _mapper);

            var sortType = "state";
            // Act 
            var result = await assetController.Get(1, 2, "", "", "", sortType);

            var query = _context.Assets
                .Include(x => x.Category)
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.State);

            var list = StaticFunctions<Asset>.Paging(query, 1, 2);

            var expected = _mapper.Map<List<ViewListAssets_AssetResponse>>(list);

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewList_ListResponse<ViewListAssets_AssetResponse>)okobjectResult.Value;

            var assetsList = resultValue.Data;

            var isSorted = assetsList.SequenceEqual(expected);
            // Assert
            Assert.Equal(assetsList.Count(), expected.Count());
            Assert.True(isSorted);
        }

        [Fact]
        public async Task GetList_ForDefaultSortedByName()
        {
            // Arrange 
            AssetsController assetController = new AssetsController(_context, _mapper);

            var sortType = "name";
            // Act 
            var result = await assetController.Get(1, 2, "", "", "", sortType);

            var query = _context.Assets
                .Include(x => x.Category)
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Name);

            var list = StaticFunctions<Asset>.Paging(query, 1, 2);

            var expected = _mapper.Map<List<ViewListAssets_AssetResponse>>(list);

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewList_ListResponse<ViewListAssets_AssetResponse>)okobjectResult.Value;

            var assetsList = resultValue.Data;

            var isSorted = assetsList.SequenceEqual(expected);
            // Assert
            Assert.Equal(assetsList.Count(), expected.Count());
            Assert.True(isSorted);
        }

        [Fact]
        public async Task GetList_ForDefaultSortedDesc()
        {
            // Arrange 
            AssetsController assetController = new AssetsController(_context, _mapper);

            var sortType = "id";
            // Act 
            var result = await assetController.Get(1, 2, "", "", "", sortType, "DESC");

            var query = _context.Assets
                .Include(x => x.Category)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.Id);

            var list = StaticFunctions<Asset>.Paging(query, 1, 2);

            var expected = _mapper.Map<List<ViewListAssets_AssetResponse>>(list);

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewList_ListResponse<ViewListAssets_AssetResponse>)okobjectResult.Value;

            var assetsList = resultValue.Data;

            var isSorted = assetsList.SequenceEqual(expected);
            // Assert
            Assert.True(isSorted);
            Assert.Equal(assetsList.Count(), expected.Count());
        }

        [Fact]
        public async Task GetList_ForDefault_InvalidPaging()
        {
            // Arrange 
            AssetsController assetController = new AssetsController(_context, _mapper);

            // Act 
            var result = await assetController.Get(-1, 2);

            var query = _context.Assets
                .Include(x => x.Category)
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Name);

            var list = StaticFunctions<Asset>.Paging(query, -1, 2);

            var expected = _mapper.Map<List<ViewListAssets_AssetResponse>>(list);

            var okobjectResult = (OkObjectResult)result.Result;

            var resultValue = (ViewList_ListResponse<ViewListAssets_AssetResponse>)okobjectResult.Value;

            var assetsList = resultValue.Data;

            var isSorted = assetsList.SequenceEqual(expected);
            // Assert
            Assert.True(isSorted);
            Assert.Equal(assetsList.Count(), expected.Count());
        }
        #endregion

        #region UpdateAsset
        [Fact]
        public async Task UpdateAsset_NotFound_ReturnBadRequest()
        {
            // Arrange 
            DateTime now = DateTime.Now;
            UpdateAssetRequest request = new UpdateAssetRequest
            {
                Name = "Laptop Asus Rog Strix",
                Specification = "Core 100, 1000 GB RAM, 200 50 GB HDD, Window 200",
                InstalledDate = now,
                State = (int)State.NotAvailable
            };

            AssetsController assetController = new AssetsController(_context, _mapper);

            // Act 
            var result = await assetController.UpdateAsset(0, request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateAsset_Success_ReturnUpdatedAsset()
        {
            // Arrange 
            DateTime now = DateTime.Now;
            UpdateAssetRequest request = new UpdateAssetRequest
            {
                Name = "Laptop Asus Rog Strix",
                Specification = "Core 100, 1000 GB RAM, 200 50 GB HDD, Window 200",
                InstalledDate = now,
                State = (int)State.NotAvailable
            };

            AssetsController assetController = new AssetsController(_context, _mapper);

            // Act 
            var response = await assetController.UpdateAsset(1, request);
            var result = ConvertOkObject<UpdateAssetResponse>(response);
            var expected = JsonConvert.SerializeObject(new UpdateAssetResponse
            {
                Id = 1,
                AssetCode = "LA10000" + 1,
                Name = "Laptop Asus Rog Strix",
                Specification = "Core 100, 1000 GB RAM, 200 50 GB HDD, Window 200",
                InstalledDate = now,
                State = State.NotAvailable,
                IsDeleted = false,
            });

            // Assert
            Assert.Equal(expected, result);
        }
        #endregion

        #region GetAssetById
        [Fact]
        public async Task GetAssetById_Success_ReturnAsset()
        {
            // Arrange 
            AssetsController assetController = new AssetsController(_context, _mapper);
            var asset = _mapper.Map<GetAssetByIdResponse>(
                await _context.Assets
                    .Where(x => x.Id == 1)
                    .FirstOrDefaultAsync()
            );

            // Act 
            var assets = _context.Assets.ToList();
            var result = await assetController.GetAssetById(1);

            string resultObject = ConvertOkObject<GetAssetByIdResponse>(result);
            string expectedObject = JsonConvert.SerializeObject(asset);

            // Assert
            Assert.Equal(resultObject, expectedObject);
        }

        [Fact]
        public async Task GetAssetById_NotFound_ReturnBadRequest()
        {
            // Arrange 
            AssetsController assetController = new AssetsController(_context, _mapper);

            // Act 
            var result = await assetController.GetAssetById(0);

            // Assert
            result.Should().BeOfType<BadRequestResult>();

        }
        #endregion

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}