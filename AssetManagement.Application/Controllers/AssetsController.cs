using AssetManagement.Contracts.Asset.Request;
using AssetManagement.Contracts.Asset.Response;
using AssetManagement.Contracts.Common;
using AssetManagement.Data.EF;
using AssetManagement.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AssetManagement.Domain.Enums.Asset;
using AssetManagement.Application.Filters;

namespace AssetManagement.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [FilterCheckIsChangeRole]
    public class AssetsController : ControllerBase
    {
        private readonly AssetManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public AssetsController(AssetManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAssetById(int id)
        {
            Asset gettingAsset = await _dbContext.Assets
                .Where(a => !a.IsDeleted && a.Id == id)
                .FirstOrDefaultAsync();

            if (gettingAsset != null)
            {
                return Ok(_mapper.Map<GetAssetByIdResponse>(gettingAsset));
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAssetAsync(CreateAssetRequest createAssetRequest)
        {
            string token = Request.Headers.Authorization;
            string userName = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name)?.Value;
            AppUser user = await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                return BadRequest(new ErrorResponseResult<string>("Invalid UserName"));
            }
            Category? category = await _dbContext.Categories.FindAsync(createAssetRequest.CategoryId);
            if (category == null)
            {
                return BadRequest(new ErrorResponseResult<string>("Invalid Category"));
            }
            Asset asset = _mapper.Map<Asset>(createAssetRequest);

            int countAsset = await _dbContext.Assets.Where(_ => _.AssetCode.StartsWith(category.Prefix)).CountAsync();
            asset.AssetCode = category.Prefix + (countAsset + 1).ToString().PadLeft(6, '0');
            asset.Category = category;
            asset.Location = user.Location;
            await _dbContext.Assets.AddAsync(asset);
            await _dbContext.SaveChangesAsync();
            return Ok(new CreateAssetResponse { Id = asset.Id, AssetCode = asset.AssetCode, Name = asset.Name, CategoryName = asset.Category.Name, State = asset.State.ToString() });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsset(int id, UpdateAssetRequest request)
        {
            Asset? updatingAsset = await _dbContext.Assets
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            try
            {
                if (updatingAsset != null)
                {
                    updatingAsset.Name = request.Name.Trim();
                    updatingAsset.Specification = request.Specification.Trim();
                    updatingAsset.InstalledDate = request.InstalledDate;
                    updatingAsset.State = (State)request.State;
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Cannot find a asset with id: {id}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponseResult<string>(ex.Message));
            }

            return Ok(_mapper.Map<UpdateAssetResponse>(updatingAsset));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsset(int id)
        {
            Asset deletingAsset = await _dbContext.Assets
                .Where(a => !a.IsDeleted && a.Id == id)
                .FirstOrDefaultAsync();

            try
            {
                if (deletingAsset != null)
                {
                    deletingAsset.IsDeleted = true;
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("The asset does not exist");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponseResult<string>(ex.Message));
            }

            return Ok(_mapper.Map<DeleteAssetReponse>(deletingAsset));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ViewListPageResult<ViewListAssetsResponse>>> Get(
            [FromQuery] int start,
            [FromQuery] int end,
            [FromQuery] string? searchString = "",
            [FromQuery] string? categoryFilter = "",
            [FromQuery] string? stateFilter = "0&1&4&",
            [FromQuery] string? sort = "assetCode",
            [FromQuery] string? order = "ASC",
            [FromQuery] string? createdId = "")
        {
            var username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            var user = _dbContext.Users.FirstOrDefault(x => x.UserName == username);
            var list = _dbContext.Assets
                .Include(x => x.Category)
                .Include(x => x.Assignments)
                .Where(x => !x.IsDeleted && x.Location == user.Location);
            if (!string.IsNullOrEmpty(searchString))
            {
                list = list.Where(x => x.Name.ToUpper().Contains(searchString.ToUpper()) || x.AssetCode.ToUpper().Contains(searchString.ToUpper()));
            }
            if (!string.IsNullOrEmpty(categoryFilter))
            {
                var arrayChar = categoryFilter.Split("&");
                var arrNumberChar = new List<int>();
                for (int i = 0; i < arrayChar.Length; i++)
                {
                    var temp = 0;
                    if (int.TryParse(arrayChar[i], out temp))
                    {
                        arrNumberChar.Add(temp);
                    }
                }
                arrayChar.Select(x =>
                {
                    var tmp = -1;
                    int.TryParse(x, out tmp);
                    return tmp;
                }).Where(a => a > -1);
                list = list.Where(x => arrNumberChar.Contains(x.CategoryId.GetValueOrDefault()));
            }
            if (!string.IsNullOrEmpty(stateFilter))
            {
                var arrayChar = stateFilter.Split("&");
                var arrNumberChar = new List<int>();
                for (int i = 0; i < arrayChar.Length; i++)
                {
                    var temp = 0;
                    if (int.TryParse(arrayChar[i], out temp))
                    {
                        arrNumberChar.Add(int.Parse(arrayChar[i]));
                    }
                }
                if(!string.IsNullOrEmpty(createdId)) {
                    list = list.Where(x => arrNumberChar.Contains((int)x.State) || x.Id == int.Parse(createdId));
                } else {
                    list = list.Where(x => arrNumberChar.Contains((int)x.State));
                }
            }
            switch (sort)
            {
                case "assetCode":
                    {
                        list = list.OrderBy(x => x.AssetCode);
                        break;
                    }
                case "name":
                    {
                        list = list.OrderBy(x => x.Name);
                        break;
                    }
                case "categoryName":
                    {
                        list = list.OrderBy(x => x.Category.Name);
                        break;
                    }
                case "state":
                    {
                        list = list.OrderBy(x => x.State);
                        break;
                    }
                case "id":
                    {
                        list = list.OrderBy(x => x.Id);
                        break;
                    }
                default:
                    {
                        list = list.OrderBy(x => x.AssetCode);
                        break;
                    }
            }

            if (order == "DESC")
            {
                list = list.Reverse();
            }

            if (!string.IsNullOrEmpty(createdId))
            {
                Asset recentlyCreatedItem = list.Where(item => item.Id == int.Parse(createdId)).AsNoTracking().FirstOrDefault();
                if (recentlyCreatedItem != null)
                {
                    list = list.Where(item => item.Id != int.Parse(createdId));

                    var sortedResultWithCreatedIdParam = StaticFunctions<Asset>.Paging(list, start, end - 1);

                    sortedResultWithCreatedIdParam.Insert(0, recentlyCreatedItem);

                    var mappedResultWithCreatedIdParam = _mapper.Map<List<ViewListAssetsResponse>>(sortedResultWithCreatedIdParam);

                    return Ok(new ViewListPageResult<ViewListAssetsResponse> { Data = mappedResultWithCreatedIdParam, Total = list.Count() + 1 });
                }
            }

            var sortedResult = StaticFunctions<Asset>.Paging(list, start, end);

            var mappedResult = _mapper.Map<List<ViewListAssetsResponse>>(sortedResult);

            return Ok(new ViewListPageResult<ViewListAssetsResponse> { Data = mappedResult, Total = list.Count() });
        }

        [HttpGet("{id}/assignmentCount")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetHistoricalAssignmentsCount(int id)
        {
            var historicalAssignmentsCount = _dbContext.Assets
                .Where(a => !a.IsDeleted && a.Id == id)
                .SelectMany(a => a.Assignments)
                .Count();

            return Ok(historicalAssignmentsCount);
        }
    }
}
