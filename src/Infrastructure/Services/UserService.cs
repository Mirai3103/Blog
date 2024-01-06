using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Blog.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Blog.Infrastructure.Services;

public class UserService(Auth0TokenManager auth0TokenManager,IConfiguration configuration)
{
    public async Task<User?> GetUserById(string userId)
    {
        var client = new ManagementApiClient(await auth0TokenManager.GetToken(), new Uri(configuration["Auth0:ManagementApi"]?? throw new InvalidOperationException("Auth0:ManagementApi is required.")));

        var user = await client.Users.GetAsync(userId);
    
  
        return user;
    }
    public Task<List<User>> GetUsers()
    {
        throw new NotImplementedException();
    }
}
