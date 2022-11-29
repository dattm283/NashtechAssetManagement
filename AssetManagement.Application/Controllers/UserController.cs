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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser(CreateUserRequest userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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

            int count = 0;
            while (duplicatename != null)
            {
                count++;
                username = (username + count.ToString());
                duplicatename = await _userManager.FindByNameAsync(username);
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
                UserName = username,
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
            [FromQuery] string userName = "")
        {
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest("Invalid Username or Log in again!");
            }
            AppUser currentUser = await _dbContext.AppUsers.FirstAsync(x => x.UserName == userName);
            if (currentUser == null)
            {
                return BadRequest("Invalid Username!");
            }
            IQueryable<AppUser> users = _dbContext.AppUsers
                                            .Where(x => x.IsDeleted == false && x.Location == currentUser.Location)
                                            .AsQueryable();

            if (!string.IsNullOrEmpty(stateFilter))
            {
                var listType = stateFilter.Split("&").ToArray();
                List<AppUser> tempData = new List<AppUser>();
                for (int i = 0; i < listType.Length - 1; i++)
                {
                    string roleName = listType[i] == "0" ? "Admin" : "Staff";
                    List<AppUser> tempUser = _userManager.GetUsersInRoleAsync(roleName).Result.ToList<AppUser>();
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

            int tempCount = 0;
            foreach (AppUser user in sortedUsers)
            {
                ViewListUser_UserResponse userData = _mapper.Map<ViewListUser_UserResponse>(user);
                userData.Id = tempCount;
                string userRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
                if (string.IsNullOrEmpty(userRole))
                {
                    continue;
                }
                userData.Type = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
                mapResult.Insert(0, userData);
                tempCount += 1;
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
            AppUser user = _dbContext.AppUsers.Where(x => x.StaffCode.Trim() == staffCode.Trim()).FirstOrDefault();

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
