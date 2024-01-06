using Auth0.ManagementApi.Models;
using Blog.Application.Articles.Queries.Dtos;

namespace Blog.Web.Extensions;

public static class Auth0UserExtension
{
    public static AuthorDto MapToAuthorDto(this User user)
    {
        return new AuthorDto()
        {
            Email = user.Email,
            Picture = user.Picture,
            FirstName = user.FirstName,
            FullName = user.FullName,
            LastName = user.LastName,
            UserId = user.UserId
        };
    }
}
