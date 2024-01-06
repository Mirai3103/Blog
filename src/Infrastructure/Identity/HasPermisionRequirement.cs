using Microsoft.AspNetCore.Authorization;
namespace Blog.Infrastructure.Identity;

public class HasPermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public HasPermissionRequirement(string permission)
    {
        Permission = permission ?? throw new ArgumentNullException(nameof(Permission));
    }
}