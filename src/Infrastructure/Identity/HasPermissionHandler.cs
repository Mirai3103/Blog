using Microsoft.AspNetCore.Authorization;

namespace Blog.Infrastructure.Identity;

public class HasPermissionHandler : AuthorizationHandler<HasPermissionRequirement>
{

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionRequirement requirement)
    {
        Console.WriteLine(context.User.Claims.Count());

        if (!context.User.HasClaim(c => c.Type == "permissions" ))
            return Task.CompletedTask;
        Console.WriteLine(context.User.Claims.Count());

        var permissions = context.User.FindAll(c => c.Type == "permissions" );
        var permissionsList = permissions.Select(x => x.Value);
        Console.WriteLine(string.Join(",",permissionsList));
        if (permissionsList.Any(s => s.Contains(requirement.Permission)))
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}
