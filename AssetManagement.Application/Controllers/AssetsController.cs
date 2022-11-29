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

namespace AssetManagement.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        // [Authorize]
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> UpdateAsset(int id, UpdateAssetRequest request)
        {
            Asset? updatingAsset = await _dbContext.Assets
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            try
            {
                if (updatingAsset != null)
                {
                    updatingAsset.Name = request.Name;
                    updatingAsset.Specification = request.Specification;
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
        [Authorize]
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
        //[Authorize]
        public async Task<ActionResult<ViewList_ListResponse<ViewListAssets_AssetResponse>>> Get([FromQuery]int start, [FromQuery]int end, [FromQuery]string? searchString="", [FromQuery]string? categoryFilter="", [FromQuery]string? stateFilter="", [FromQuery]string? sort="name", [FromQuery]string? order="ASC", [FromQuery]string? createdId="")
        {
            var list = _dbContext.Assets
                .Include(x=>x.Category)
                .Where(x=>!x.IsDeleted);
            if (!string.IsNullOrEmpty(searchString))
            {
                list = list.Where(x => x.Name.ToUpper().Contains(searchString.ToUpper()) || x.AssetCode.ToUpper().Contains(searchString.ToUpper()));
            }
            if(!string.IsNullOrEmpty(categoryFilter))
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
            if(!string.IsNullOrEmpty(stateFilter))
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
                list = list.Where(x=> arrNumberChar.Contains((int)x.State));
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
            }

            if (order == "DESC")
            {
                list = list.Reverse();
            }

            if (!string.IsNullOrEmpty(createdId))
            {
                var newList = list.Where(item => item.Id == int.Parse(createdId));
                list = list.Where(item => item.Id != int.Parse(createdId));
                list = newList.Concat(list);
            }

            var sortedResult = StaticFunctions<Asset>.Paging(list, start, end);

            var mappedResult = _mapper.Map<List<ViewListAssets_AssetResponse>>(sortedResult);

            return Ok(new ViewList_ListResponse<ViewListAssets_AssetResponse> { Data = mappedResult, Total = list.Count() });
        }
    }
}
