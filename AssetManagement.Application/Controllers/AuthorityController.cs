using AssetManagement.Contracts.Authority.Request;
using AssetManagement.Contracts.Authority.Response;
using AssetManagement.Contracts.Common;
using AssetManagement.Data.EF;
using AssetManagement.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AssetManagement.Application.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthorityController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        private readonly AssetManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public AuthorityController(UserManager<AppUser> userManager,
            IConfiguration config, 
            AssetManagementDbContext dbContext,
            IMapper mapper)
        {
            _userManager = userManager;
            _config = config;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpPost("auth/token/")]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return BadRequest(new ErrorResponseResult<string>("Account does not exist."));
            }
            else if(user.IsDeleted)
            {
                return BadRequest(new ErrorResponseResult<string>("Account has been banned."));
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
            {
                return BadRequest(new ErrorResponseResult<string>("No match for username and/or password."));
            }

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            //var role = await _dbContext.AppRoles.FindAsync(user.RoleId);

            return Ok(new SuccessResponseResult<LoginResponse>(new LoginResponse { Token = CreateToken(user, request.Username, role), Role = role }));
        }

        [HttpGet("auth/user-profile/")]
        [Authorize]
        public async Task<IActionResult> GetUserProfile()
        {
            var result = await _userManager.FindByNameAsync(User.Identity.Name);
            var data = _mapper.Map<UserResponse>(result);

            return Ok(data);
        }

        [HttpPost("auth/change-password/")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            user.IsLoginFirstTime = false;

            await _userManager.UpdateAsync(user);

            return Ok(new SuccessResponseResult<string>("Change password success!"));
        }

        private string CreateToken(AppUser user, string username, string role)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = GetClaims(user, username, role);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private IList<Claim> GetClaims(AppUser user, string username, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IList<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken
                (issuer: _config["JwtSettings:validIssuer"],
                audience: _config["JwtSettings:validIssuer"],
                claims: claims,
                expires: DateTime.Now.AddHours(int.Parse(_config["JwtSettings:expires"])),
                signingCredentials: signingCredentials);
            return tokenOptions;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }
    }
}
