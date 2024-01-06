using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Blog.Infrastructure.Services;

public class Auth0TokenManager
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration configuration;

    public Auth0TokenManager(IHttpClientFactory httpClientFactory,IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        this.configuration = configuration;
    }

    public async Task<string> GetToken()
    {
        var oldTokenExpiresString = configuration["Auth0:TokenExpires"];
        if (oldTokenExpiresString != null && DateTime.Parse(oldTokenExpiresString) > DateTime.UtcNow)
        {
            return configuration["Auth0:Token"] ?? throw new Exception("Something went wrong");
        }
        var client = _httpClientFactory.CreateClient("Auth0");
        var domain = configuration["Auth0:Domain"]??throw new Exception("Auth0:Domain is not configured");
        var url = $"https://{domain}/";
        client.BaseAddress = new Uri(url);
        var response = await client.PostAsync("oauth/token", new FormUrlEncodedContent(new Dictionary<string, string>()
        {
            {"grant_type", "client_credentials"},
            {"client_id", configuration["Auth0:ClientId"]??throw new Exception("Auth0:ClientId is not configured")},
            {"client_secret", configuration["Auth0:ClientSecret"]??throw new Exception("Auth0:ClientSecret is not configured")},
            {"audience", configuration["Auth0:ManagementApiAudience"]??throw new Exception("Auth0:ManagementApiAudience is not configured")}
        }));
        var content = await response.Content.ReadAsStringAsync()!;
        var token = JsonConvert.DeserializeObject<Dictionary<string, string>>(content)??throw new Exception("Something went wrong");
        
        configuration["Auth0:Token"] = token["access_token"]??throw new Exception("Something went wrong");
        configuration["Auth0:TokenExpires"] = DateTime.UtcNow.AddSeconds(int.Parse(token["expires_in"]??throw new Exception("Something went wrong"))).ToString("o");
        return configuration["Auth0:Token"]??throw new Exception("Something went wrong");
    }
}
