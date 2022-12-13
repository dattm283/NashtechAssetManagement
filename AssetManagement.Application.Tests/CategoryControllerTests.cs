using Microsoft.EntityFrameworkCore;
using AssetManagement.Data.EF;
using AutoMapper;
using AssetManagement.Contracts.AutoMapper;
using AssetManagement.Domain.Models;
using Xunit;
using AssetManagement.Contracts.Category.Request;
using Microsoft.AspNetCore.Mvc;
using AssetManagement.Application.Controllers;
using AssetManagement.Contracts.Category.Response;
using AssetManagement.Contracts.Common;
using AssetManagement.Application.Tests.TestHelper;

#nullable disable
namespace AssetManagement.Application.Tests
{

    public class CategoryControllerTests : IAsyncDisposable
    {
        private readonly DbContextOptions _options;
        private readonly AssetManagementDbContext _context;
        private readonly IMapper _mapper;
        private List<Category> _categories;
        public CategoryControllerTests()
        {
            //Create InMemory dbcontext
            _options = new DbContextOptionsBuilder<AssetManagementDbContext>().UseInMemoryDatabase("CategoryTestDB").Options;
            _context = new AssetManagementDbContext(_options);
            //Create mapper using CategoryProfile
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new CategoryProfile())).CreateMapper();

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        #region GetCategory
        [Fact]
        public async Task Get_SuccessAsync()
        {
            //ARRANGE
            CategoryController controller = new(_mapper, _context);

            //ACT
            ActionResult<ViewListPageResult<GetCategoryResponse>> response = await controller.GetAsync();
            IActionResult result = response.Result;
            ViewListPageResult<GetCategoryResponse> data = (ViewListPageResult<GetCategoryResponse>)((OkObjectResult)result).Value;
            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equivalent(_mapper.Map<List<GetCategoryResponse>>(_categories), data.Data);
        }
        #endregion

        #region CreateCategory
        [Theory]
        [InlineData("Mouse", "Ms")]
        [InlineData("Cable", "CA")]
        public async Task Create_SuccessAsync(string name, string prefix)
        {
            //ARRANGE
            CreateCategoryRequest request = new() { Name = name, Prefix = prefix };
            CategoryController controller = new(_mapper, _context);

            //ACT
            var result = await controller.CreateAsync(request);
            Category data = (Category)((ObjectResult)result).Value;
            Category newCat = _context.Categories.LastOrDefault();

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(newCat);
            Assert.Equivalent(newCat, data);
        }

        [Theory]
        [InlineData(null, "KY")]
        [InlineData("Kettle", null)]
        [InlineData(null, null)]
        public async Task Create_Badrequest_ModelState_InvalidAsync(string name, string prefix)
        {
            //ARRANGE
            CreateCategoryRequest request = new() { Name = name, Prefix = prefix };
            CategoryController controller = new(_mapper, _context);
            //Assume controller has modelstate errors to simulate data annotations
            if (name == null) controller.ModelState.AddModelError("name", "Please enter Category Name");
            if (prefix == null) controller.ModelState.AddModelError("prefix", "Please enter Category Prefix");

            //ACT
            IActionResult result = await controller.CreateAsync(request);
            SerializableError error = (SerializableError)((ObjectResult)result).Value;

            //ASSERT
            Assert.IsType<BadRequestObjectResult>(result);
            if (error.ContainsKey("name"))
            {
                Assert.Equal("Please enter Category Name", ((string[])error["name"])[0]);
            }

            if (error.ContainsKey("prefix"))
            {
                Assert.Equal("Please enter Category Prefix", ((string[])error["prefix"])[0]);
            }
        }

        [Theory]
        [InlineData("Personal Computer", "KY")]
        [InlineData("laptop", "LA")]
        [InlineData("mOnitoR", "Mn")]
        public async Task Create_BadRequest_UniqueNameAsync(string name, string prefix)
        {
            //ARRANGE
            CreateCategoryRequest request = new() { Name = name, Prefix = prefix };
            CategoryController controller = new(_mapper, _context);

            //ACT
            IActionResult result = await controller.CreateAsync(request);
            string message = ((ObjectResult)result).Value.ToString();
            //ASSERT
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Category is already existed. Please enter a different category", message);
        }

        [Theory]
        [InlineData("Mouse", "MO")]
        [InlineData("Large Cable", "lA")]
        [InlineData("Kibble", "pc")]
        public async Task Create_BadRequest_UniquePrefixAsync(string name, string prefix)
        {
            //ARRANGE
            CreateCategoryRequest request = new() { Name = name, Prefix = prefix };
            CategoryController controller = new(_mapper, _context);

            //ACT
            IActionResult result = await controller.CreateAsync(request);
            string message = ((ObjectResult)result).Value.ToString();
            //ASSERT
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Prefix is already existed. Please enter a different prefix", message);
        }

        [Theory]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "AA")]
        public async Task Create_BadRequest_CategoryNameOverMaxLength(string name, string prefix)
        {
            //ARRANGE
            CreateCategoryRequest request = new() { Name = name, Prefix = prefix };
            CategoryController controller = new(_mapper, _context);

            //ACT
            IActionResult result = await controller.CreateAsync(request);
            string message = ((ObjectResult)result).Value.ToString();
            //ASSERT
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("New category's name no longer than 100 characters", message);
        }

        [Theory]
        [InlineData("Mouse", "MOMOMO")]
        public async Task Create_BadRequest_CategoryPrefixOverMaxLength(string name, string prefix)
        {
            //ARRANGE
            CreateCategoryRequest request = new() { Name = name, Prefix = prefix };
            CategoryController controller = new(_mapper, _context);

            //ACT
            IActionResult result = await controller.CreateAsync(request);
            string message = ((ObjectResult)result).Value.ToString();
            //ASSERT
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("New category's prefix no longer than 5 characters", message);
        }

        [Fact]
        public async Task Create_BadRequest_ExceptionAsync()
        {
            //ARRANGE
            CreateCategoryRequest request = new() { Name = "PowerCord", Prefix = "PwC" };
            //Add null as mapper to cause exception
            CategoryController controller = new(null, _context);

            //ACT
            IActionResult result = await controller.CreateAsync(request);
            string message = ((ObjectResult)result).Value.ToString();
            //ASSERT
            Assert.IsType<BadRequestObjectResult>(result);
        }
        #endregion

        //Clean up after tests
        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await _context.Database.CloseConnectionAsync();
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }
    }
}