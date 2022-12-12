using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace AssetManagement.Application.Filters
{
    public class FilterCheckIsChangeRole : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userString = context.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var isValid = StaticValues.Usernames.Contains(userString);
            if (!isValid)
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Your role has been changed" });
            }
        }
    }
}
