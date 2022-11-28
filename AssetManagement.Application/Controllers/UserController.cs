using AssetManagement.Contracts.Asset.Response;
using AssetManagement.Contracts.Common;
using AssetManagement.Contracts.User.Response;
using AssetManagement.Data.EF;
using AssetManagement.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AssetManagementDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(AssetManagementDbContext dbContext, UserManager<AppUser> userManager, IMapper mapper)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<ViewList_ListResponse<ViewListUser_UserResponse>>> GetAllUser(
            [FromQuery] int start,
            [FromQuery] int end,
            [FromQuery] string? typeFilter = "",
            [FromQuery] string? searchString = "",
            [FromQuery] string? sort = "staffCode",
            [FromQuery] string? order = "ASC")
        {
            IQueryable<AppUser> users = _dbContext.AppUsers
                                            .Where(x => x.IsDeleted == false)
                                            .AsQueryable();

            if (!string.IsNullOrEmpty(typeFilter))
            {
                List<string> listType = typeFilter.Split("&").ToList();
                foreach (string type in listType)
                {
                    users = _userManager.GetUsersInRoleAsync(type).Result.AsQueryable();
                }
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(x => (x.FirstName + ' ' + x.LastName).Contains(searchString)
                                        || x.StaffCode.Contains(searchString));
            }

            switch (sort)
            {
                case "staffCode":
                    {
                        users = users.OrderBy(x => x.StaffCode);
                        break;
                    }
                case "fullName":
                    {
                        users = users.OrderBy(x => x.FirstName + ' ' + x.LastName);
                        break;
                    }
                case "userName":
                    {
                        users = users.OrderBy(x => x.UserName);
                        break;
                    }
                case "joinedDate":
                    {
                        users = users.OrderBy(x => x.CreatedDate);
                        break;
                    }
                case "type":
                    {
                        users = users.OrderBy(x => _userManager.GetRolesAsync(x).Result.FirstOrDefault());
                        break;
                    }
            }

            if (order == "DESC")
            {
                users = users.Reverse();
            }

            List<AppUser> sortedUsers = StaticFunctions<AppUser>.Paging(users, start, end);

            List<ViewListUser_UserResponse> mapResult = new List<ViewListUser_UserResponse>();

            foreach (AppUser user in sortedUsers)
            {
                ViewListUser_UserResponse userData = _mapper.Map<ViewListUser_UserResponse>(user);
                userData.Type = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
                mapResult.Insert(0, userData);
            }

            return Ok(new ViewList_ListResponse<ViewListUser_UserResponse>
            {
                Data = mapResult,
                Total = mapResult.Count
            });
        }

        [HttpGet("{staffCode}")]
        public async Task<ActionResult<ViewDetailUser_UserResponse>> GetSingleUser([FromRoute] string staffCode)
        {
            AppUser user = _dbContext.AppUsers.Where(x => x.StaffCode.Trim()==staffCode.Trim()).FirstOrDefault();

            if (user == null)
            {
                return BadRequest(new ErrorResponseResult<string>("Invalid StaffCode"));
            }

            ViewDetailUser_UserResponse result = _mapper.Map<ViewDetailUser_UserResponse>(user);
            result.Type = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            return Ok(result);
        }
    }
}
