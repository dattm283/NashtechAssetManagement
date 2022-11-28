using AssetManagement.Contracts.Common;
using AssetManagement.Contracts.User.Request;
using AssetManagement.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Application.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost("user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser(UserRequest userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //auto generate staff code
            var staffCode = "SD0001";
            int total = await _userManager.Users.CountAsync();
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

            var duplicatename = await _userManager.Users.FirstOrDefaultAsync(p => p.UserName == username);

            int count = 0;
            while (duplicatename != null)
            {
                count++;
                username = (username + count.ToString());
                duplicatename = await _userManager.Users.FirstOrDefaultAsync(p => p.UserName == username);
            }

            //auto generate password
            string dateOfBirth = userRequest.Dob.ToString("ddMMyyyy");
            string password = $"{username}@{dateOfBirth}";

            //get location from current admin
            var admin = await _userManager.FindByNameAsync(User.Identity.Name);

            var user = new AppUser
            {
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                Dob = userRequest.Dob,
                CreatedDate = userRequest.CreatedDate,
                Gender = userRequest.Gender,
                RoleId = new Guid(userRequest.RoleId),
                UserName = username,
                Location = admin.Location,
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return Ok(new SuccessResponseResult<string>("Create user success!"));
            }

            return BadRequest(new ErrorResponseResult<bool>("Create user unsuccessfully!"));
        }
    }
}
