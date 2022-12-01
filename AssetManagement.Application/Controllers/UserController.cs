using AssetManagement.Contracts.Authority.Request;
using AssetManagement.Contracts.Common;
using AssetManagement.Contracts.User.Request;
using AssetManagement.Data.EF;
using AssetManagement.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        private readonly AssetManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserController(UserManager<AppUser> userManager,
            IConfiguration config,
            AssetManagementDbContext dbContext,
            IMapper mapper)
        {
            _userManager = userManager;
            _config = config;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponseResult<string>("Invalid password"));
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (!(await _userManager.CheckPasswordAsync(user, request.CurrentPassword)))
            {
                return BadRequest(new ErrorResponseResult<string>("Password is incorrect"));
            }

            if(request.CurrentPassword == request.NewPassword)
            {
                return BadRequest(new ErrorResponseResult<string>("New password must be different"));
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
