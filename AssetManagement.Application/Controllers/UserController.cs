using AssetManagement.Contracts.Common;
using AssetManagement.Contracts.User.Request;
﻿using AssetManagement.Contracts.Asset.Response;
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
using AssetManagement.Domain.Enums.AppUser;
using System.Collections.Generic;

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
            string[] splitFirstName = userRequest.FirstName.Split(' ');
            string fullFirstName = "";
            foreach (string slice in splitFirstName)
            {
                if (slice.Length > 0)
                {
                    fullFirstName += slice.ToString().ToLower();
                }
            }
            string[] splitlastname = userRequest.LastName.Split(' ');
            string fullLastName = "";
            foreach (string slice in splitlastname)
            {
                if (slice.Length > 0)
                {
                    fullLastName += slice.ToString().ToLower();
                }
            }

            string username = fullFirstName + fullLastName;

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
            string password = $"{username}@{dateOfBirth}";

            var user = new AppUser
            {
                StaffCode = staffCode,
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
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
                return Ok(new CreateUserResponse { Id = user.Id, UserName = user.UserName, FirstName = user.FirstName, LastName = user.LastName, Dob = user.Dob, CreatedDate = user.CreatedDate });
            }

            return BadRequest(new ErrorResponseResult<bool>("Create user unsuccessfully!"));
        }

        [HttpGet]
        public async Task<ActionResult<ViewList_ListResponse<ViewListUser_UserResponse>>> GetAllUser(
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
            IQueryable<AppUser> users = _dbContext.AppUsers
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
            }

            if (order == "DESC")
            {
                users = users.Reverse();
            }

            if (!string.IsNullOrEmpty(createdId))
            {
                createdId = createdId.Substring(1, 36);
                AppUser recentlyCreatedItem = users.Where(item => item.Id == Guid.Parse(createdId)).AsNoTracking().FirstOrDefault();
                users = users.Where(item => item.Id != Guid.Parse(createdId));

                var sortedResultWithCreatedIdParam = StaticFunctions<AppUser>.Paging(users, start, end - 1);

                sortedResultWithCreatedIdParam.Insert(0, recentlyCreatedItem);

                var mappedResultWithCreatedIdParam = _mapper.Map<List<ViewListUser_UserResponse>>(sortedResultWithCreatedIdParam);

                return Ok(new ViewList_ListResponse<ViewListUser_UserResponse> { Data = mappedResultWithCreatedIdParam, Total = users.Count() });
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

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletingUser = await _dbContext.AppUsers.FirstOrDefaultAsync(x => x.Id == id);
            var userName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            if (deletingUser != null)
            {
                if (deletingUser.UserName == userName)
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
