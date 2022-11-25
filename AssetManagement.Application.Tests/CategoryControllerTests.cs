using Microsoft.EntityFrameworkCore;
using AssetManagement.Data.EF;
using AutoMapper;
using AssetManagement.Contracts.AutoMapper;
using AssetManagement.Domain.Models;
using Xunit;
using AssetManagement.Contracts.Category.Response;
using AssetManagement.Contracts.Category.Request;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using AssetManagement.Contracts.Asset.Response;

#nullable disable
namespace AssetManagement.Application.Controllers.Tests
{
    public class CategoryControllerTests : IDisposable
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

            SeedData();
        }

        #region GetCategory
        //[Fact]
        //public async Task Get_SuccessAsync()
        //{
        //    //ARRANGE
        //    CategoryController controller = new(_mapper, _context);

        //    //ACT
        //    ViewList_ListResponse<GetCategoryResponse> result = await controller.GetAsync();

        //    //ASSERT
        //    Assert.NotNull(result);
        //    Assert.NotEmpty(result);
        //    Assert.Equivalent(_mapper.Map<List<GetCategoryResponse>>(_categories), result);
        //}
        #endregion

        #region CreateCategory
        [Theory]
        [InlineData("Mouse", "Ms")]
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
        [InlineData("KeyBoard", "KY")]
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
        [InlineData("Lube Tube", "lT")]
        [InlineData("Kibble", "kb")]
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

        #region DataSeed
        private void SeedData()
        {
            _context.Database.EnsureDeleted();
            _categories = new()
            {
                new (){Name = "Laptop", Prefix = "LT", IsDeleted = false },
                new (){Name = "Monitor", Prefix = "MO", IsDeleted = false },
                new (){Name = "Keyboard", Prefix = "KB", IsDeleted = false },
            };

            _context.Categories.AddRange(_categories);
            _context.SaveChanges();
        }
        #endregion

        //Clean up after tests
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}