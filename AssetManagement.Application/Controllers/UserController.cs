using AssetManagement.Contracts.Common;
using AssetManagement.Contracts.User.Request;
using AssetManagement.Contracts.Asset.Response;
using AssetManagement.Contracts.Authority.Request;
using AssetManagement.Contracts.Common;
using AssetManagement.Contracts.User.Response;
using AssetManagement.Data.EF;
using AssetManagement.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AssetManagement.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AssetManagement.Domain.Enums.AppUser;
using System.Collections.Generic;
using Diacritics.Extensions;
using AssetManagement.Application.Filters;

namespace AssetManagement.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [FilterCheckIsChangeRole]
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

        private int GetAge(DateTime bornDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - bornDate.Year;
            if (bornDate > today.AddYears(-age))
                age--;

            return age;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser(CreateUserRequest userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int age = GetAge(userRequest.Dob);
            if (age < 18)
            {
                return BadRequest(new ErrorResponseResult<bool>("User is under 18. Please select a different date"));
            }

            if (userRequest.JoinedDate.DayOfWeek == DayOfWeek.Saturday ||
                userRequest.JoinedDate.DayOfWeek == DayOfWeek.Sunday)
            {
                return BadRequest(new ErrorResponseResult<bool>("Joined date is Saturday or Sunday. Please select a different date"));
            }

            DateTime condition = userRequest.Dob;
            condition.AddYears(18);
            if (userRequest.JoinedDate < condition)
            {
                return BadRequest(new ErrorResponseResult<bool>("User under the age of 18 may not join company. Please select a different date"));
            }

            //auto generate staff code
            var staffCode = "SD0001";
            int total = await _dbContext.Users.CountAsync();
            if (total >= 0)
            {
                total++;
                staffCode = "SD" + total.ToString().PadLeft(4, '0');
            }

            //auto generate username
            string[] splitFirstName = userRequest.FirstName.Trim().Split(' ');
            string fullFirstName = "";
            foreach (string slice in splitFirstName)
            {
                if (slice.Length > 0)
                {
                    fullFirstName += slice.ToString().ToLower();
                }
            }
            string[] splitlastname = userRequest.LastName.Trim().Split(' ');
            string fullLastName = "";
            foreach (string slice in splitlastname)
            {
                if (slice.Length > 0)
                {
                    fullLastName += slice[0].ToString().ToLower();
                }
            }

            string username = fullFirstName + fullLastName;
            username = username.RemoveDiacritics();

            var duplicatename = await _userManager.FindByNameAsync(username);

            string newUsername = username;
            int count = 0;
            while (duplicatename != null)
            {
                count++;
                newUsername = (username + count.ToString());
                duplicatename = await _userManager.FindByNameAsync(newUsername);
            }

            //auto generate password
            string dateOfBirth = userRequest.Dob.ToString("ddMMyyyy");
            string password = $"{newUsername}@{dateOfBirth}";


            fullFirstName = "";
            foreach (string slice in splitFirstName)
            {
                if (slice.Length > 0)
                {
                    fullFirstName += slice.ToString() + " ";
                }
            }

            fullLastName = "";
            foreach (string slice in splitlastname)
            {
                if (slice.Length > 0)
                {
                    fullLastName += slice.ToString() + " ";
                }
            }
            var user = new AppUser
            {
                StaffCode = staffCode,
                FirstName = fullFirstName.Trim(),
                LastName = fullLastName.Trim(),
                IsLoginFirstTime = true,
                Dob = userRequest.Dob,
                CreatedDate = userRequest.JoinedDate,
                Gender = Enum.Parse<UserGender>(userRequest.Gender),
                UserName = newUsername,
                Location = Enum.Parse<AppUserLocation>(User.FindFirst(ClaimTypes.Locality).Value.ToString()),
            };

            var result = await _userManager.CreateAsync(user, password);
            var resultRole = await _userManager.AddToRoleAsync(user, userRequest.Role);

            if (result.Succeeded && resultRole.Succeeded)
            {
                return Ok(new CreateUserResponse { Id = user.Id, StaffCode = user.StaffCode, UserName = user.UserName, FirstName = user.FirstName, LastName = user.LastName, Dob = user.Dob, CreatedDate = user.CreatedDate });
            }

            return BadRequest(new ErrorResponseResult<bool>("Create user unsuccessfully!"));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ViewListPageResult<ViewListUser_UserResponse>>> GetAllUser(
            [FromQuery] int start,
            [FromQuery] int end,
            [FromQuery] string? stateFilter = "",
            [FromQuery] string? searchString = "",
            [FromQuery] string? sort = "staffCode",
            [FromQuery] string? order = "ASC",
            [FromQuery] string? createdId = "")
        {
            string userName = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name)?.Value;
            AppUser currentUser = await _dbContext.AppUsers.FirstAsync(x => x.UserName == userName);
            IQueryable<AppUser> users = _dbContext.AppUsers.Include(u => u.AssignedToAssignments.Where(a => !a.IsDeleted && a.State != Domain.Enums.Assignment.State.Returned))
                                                           .Where(x => x.IsDeleted == false && x.Location == currentUser.Location);

            if (!string.IsNullOrEmpty(stateFilter))
            {
                var listType = stateFilter.Split("&");
                List<AppUser> tempData = new List<AppUser>();
                for (int i = 0; i < listType.Length - 1; i++)
                {
                    string roleName = listType[i] == "Admin" ? "Admin" : "Staff";
                    IQueryable<AppUser> tempUser = _userManager.GetUsersInRoleAsync(roleName).Result.AsQueryable<AppUser>();
                    tempData.AddRange(tempUser);
                }
                users = users.Where(x => tempData.Contains(x));
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(x => (x.FirstName.ToUpper() + ' ' + x.LastName.ToUpper()).Contains(searchString.ToUpper())
                                        || x.StaffCode.ToUpper().Contains(searchString.ToUpper()));
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
                        var userWithRole = from user in users
                                           join userRole in _dbContext.UserRoles
                                           on user.Id equals userRole.UserId
                                           join role in _dbContext.Roles
                                           on userRole.RoleId equals role.Id
                                           orderby role.NormalizedName
                                           select user;
                        users = userWithRole;
                        break;
                    }
                default:
                    {
                        users = users.OrderBy(x => x.StaffCode);
                        break;
                    }
            }

            if (order == "DESC")
            {
                users = users.Reverse();
            }

            if (!string.IsNullOrEmpty(createdId))
            {
                createdId = createdId.Substring(1, 36);
                AppUser recentlyCreatedItem = users.Where(item => item.Id == Guid.Parse(createdId)).AsNoTracking().FirstOrDefault();
                if (recentlyCreatedItem != null)
                {
                    users = users.Where(item => item.Id != Guid.Parse(createdId));

                    var sortedResultWithCreatedIdParam = StaticFunctions<AppUser>.Paging(users, start, end - 1);

                    sortedResultWithCreatedIdParam.Insert(0, recentlyCreatedItem);

                    List<ViewListUser_UserResponse> mapResultWithCreatedIdParam = new List<ViewListUser_UserResponse>();

                    //int tempCount = 0;
                    foreach (AppUser user in sortedResultWithCreatedIdParam)
                    {
                        ViewListUser_UserResponse userData = _mapper.Map<ViewListUser_UserResponse>(user);
                        //userData.Id = tempCount;
                        string userRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
                        if (string.IsNullOrEmpty(userRole))
                        {
                            continue;
                        }
                        userData.Type = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
                        mapResultWithCreatedIdParam.Insert(0, userData);
                        //tempCount += 1;
                    }
                    mapResultWithCreatedIdParam.Reverse();

                    return Ok(new ViewListPageResult<ViewListUser_UserResponse> { Data = mapResultWithCreatedIdParam, Total = users.Count() + 1 });
                }

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
            mapResult.Reverse();

            return Ok(new ViewListPageResult<ViewListUser_UserResponse>
            {
                Data = mapResult,
                Total = users.Count()
            });
        }

        [HttpGet("{staffCode}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SuccessResponseResult<ViewDetailUser_UserResponse>>> GetSingleUser([FromRoute] string staffCode)
        {
            AppUser user = _dbContext.AppUsers.Where(x => x.StaffCode.Trim() == staffCode.Trim()).FirstOrDefault();

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

        [Authorize(Roles = "Admin")]
        [HttpPut("{StaffCode}")]
        public async Task<IActionResult> UpdateUserAsync(string staffCode, UpdateUserRequest request)
        {
            if (ModelState.IsValid)
            {
                if (request.Dob > DateTime.Now.AddYears(-18))
                {
                    return BadRequest("User is under 18. Please select a different date");
                }

                if (request.JoinedDate < request.Dob.AddYears(18))
                {
                    return BadRequest("User under the age 18 may not join the company. Please select a different date");
                }

                if (request.JoinedDate.DayOfWeek == DayOfWeek.Saturday || request.JoinedDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    return BadRequest("Joined date is Saturday or Sunday. Please select a different date");
                }

                AppUser? user = _dbContext.AppUsers.FirstOrDefault(u => u.StaffCode == staffCode && !u.IsDeleted);
                AppRole? role = _dbContext.AppRoles.FirstOrDefault(r => r.Name == request.Type);

                if (user != null && role != null)
                {
                    try
                    {
                        IdentityUserRole<Guid>? roleKey = _dbContext.UserRoles.FirstOrDefault(ur => ur.UserId == user.Id);

                        user.Dob = request.Dob;
                        user.CreatedDate = request.JoinedDate;
                        user.Gender = (Domain.Enums.AppUser.UserGender)request.Gender;
                        user.ModifiedDate = DateTime.Now;

                        if (roleKey.RoleId != role.Id)
                        {
                            _dbContext.UserRoles.Remove(roleKey);
                            IdentityUserRole<Guid> newRoleKey = new() { UserId = user.Id, RoleId = role.Id };
                            await _dbContext.UserRoles.AddAsync(newRoleKey);
                        }
                        //remove username from static list
                        StaticValues.Usernames.Remove(user.UserName);

                        await _dbContext.SaveChangesAsync();
                        UpdateUserResponse response = _mapper.Map<UpdateUserResponse>(user);
                        response.Type = request.Type;
                        return Ok(response);
                    }

                    catch (Exception e) { return BadRequest(e.Message); }
                }

                return NotFound();
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{staffCode}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string staffCode)
        {
            var deletingUser = await _dbContext.AppUsers.Include(u => u.AssignedToAssignments.Where(a => a.State != Domain.Enums.Assignment.State.Returned))
                                                        .FirstOrDefaultAsync(x => x.StaffCode == staffCode);
            var userName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            if (deletingUser != null)
            {
                if (deletingUser.UserName == userName)
                {
                    return BadRequest(new ErrorResponseResult<string>("You can't delete yourself"));
                }
                if (deletingUser.AssignedToAssignments.Count() > 0)
                {
                    return BadRequest(new ErrorResponseResult<string>("There are valid assignments belonging to this user. Please close all assignments before disabling user."));
                }
                deletingUser.IsDeleted = true;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                return BadRequest(new ErrorResponseResult<string>("Can't find this user"));
            }
            StaticValues.Usernames.Remove(deletingUser.UserName);   
            return Ok(_mapper.Map<DeleteUserResponse>(deletingUser));
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (!(await _userManager.CheckPasswordAsync(user, request.CurrentPassword)))
            {
                return BadRequest(new ErrorResponseResult<string>("Password is incorrect"));
            }

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(new ErrorResponseResult<string>(result.Errors.ToString()));
            }

            await _userManager.UpdateAsync(user);

            return Ok(new SuccessResponseResult<string>("Change password success!"));
        }
    }
}
