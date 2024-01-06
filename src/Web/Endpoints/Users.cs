using System.Security.Claims;
using Auth0.AspNetCore.Authentication;
using Auth0.ManagementApi.Models;
using Blog.Infrastructure.Identity;
using Blog.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Endpoints;

public class Users : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app
            .MapGet(GetProfile, "whoami");
    }
        [Authorize("CanCreateArticle")]
    public async Task<  IResult> GetProfile(HttpContext context,UserService userService)
    {
        var user = context.User;
        var claims = user.Claims.Select(c => new { c.Type, c.Value });
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var auth0User =await userService.GetUserById(userId);
        return Results.Ok(claims);
    }
    

}
