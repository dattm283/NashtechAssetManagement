using AssetManagement.Contracts.Asset.Response;
using AssetManagement.Contracts.Common;
using AssetManagement.Contracts.User.Response;
using AssetManagement.Data.EF;
using AssetManagement.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            [FromQuery] string? stateFilter = "",
            [FromQuery] string? searchString = "",
            [FromQuery] string? sort = "staffCode",
            [FromQuery] string? order = "ASC")
        {
            string userName = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name)?.Value;
            AppUser currentUser = await _dbContext.AppUsers.FirstAsync(x => x.UserName == userName);
            IQueryable<AppUser> users = _dbContext.AppUsers
                                            .Where(x => x.IsDeleted==false && x.Location==currentUser.Location);

            if (!string.IsNullOrEmpty(stateFilter))
            {
                var listType = stateFilter.Split("&");
                List<AppUser> tempData = new List<AppUser>();
                for (int i=0; i<listType.Length-1; i++)
                {
                    string roleName = listType[i] == "Admin" ? "Admin" : "Staff";
                    IQueryable<AppUser> tempUser = _userManager.GetUsersInRoleAsync(roleName).Result.AsQueryable<AppUser>();
                    tempData.AddRange(tempUser);
                }
                users = users.Where(x => tempData.Contains(x));
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
                        var adminUsers = _userManager.GetUsersInRoleAsync("Admin").Result;
                        var staffUsers = _userManager.GetUsersInRoleAsync("Staff").Result;
                        List<AppUser> listUsers_1 = await users.Where(x => adminUsers.Contains(x)).ToListAsync();
                        List<AppUser> listUsers_2 = await users.Where(x => staffUsers.Contains(x)).ToListAsync();
                        listUsers_1.AddRange(listUsers_2);
                        users = listUsers_1.AsQueryable().AsNoTracking();
                        break;
                    }
            }

            if (order == "DESC")
            {
                users = users.Reverse();
            }

            List<AppUser> sortedUsers = StaticFunctions<AppUser>.Paging(users, start, end);

            List<ViewListUser_UserResponse> mapResult = new List<ViewListUser_UserResponse>();

            //int tempCount = 0;
            foreach (AppUser user in sortedUsers)
            {
                ViewListUser_UserResponse userData = _mapper.Map<ViewListUser_UserResponse>(user);
                //userData.Id = tempCount;
                string userRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
                if (string.IsNullOrEmpty(userRole))
                {
                    continue;
                }
                userData.Type = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
                mapResult.Insert(0, userData);
                //tempCount += 1;
            }

            return Ok(new ViewList_ListResponse<ViewListUser_UserResponse>
            {
                Data = mapResult,
                Total = mapResult.Count
            });
        }

        [HttpGet("{staffCode}")]
        public async Task<ActionResult<SuccessResponseResult<ViewDetailUser_UserResponse>>> GetSingleUser([FromRoute] string staffCode)
        {
            AppUser user = _dbContext.AppUsers.Where(x => x.StaffCode.Trim()==staffCode.Trim()).FirstOrDefault();

            if (user == null)
            {
                return BadRequest(new ErrorResponseResult<string>("Invalid StaffCode"));
            }

            ViewDetailUser_UserResponse result = _mapper.Map<ViewDetailUser_UserResponse>(user);
            result.Type = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            return Ok(new SuccessResponseResult<ViewDetailUser_UserResponse>
            {
                Result = result,
                IsSuccessed = true
            });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletingUser = await _dbContext.AppUsers.FirstOrDefaultAsync(x => x.Id == id);
            var userName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            if (deletingUser != null)
            {
                if(deletingUser.UserName == userName)
                {
                    return BadRequest(new ErrorResponseResult<string>("You can't delete yourself"));
                }
                deletingUser.IsDeleted = true;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                return BadRequest(new ErrorResponseResult<string>("Can't find this user"));
            }

            return Ok(_mapper.Map<DeleteUserResponse>(deletingUser));
        }
    }
}
