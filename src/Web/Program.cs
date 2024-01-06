using System.Security.Claims;
using Auth0.AspNetCore.Authentication;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddKeyVaultIfConfigured(builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.IncludeErrorDetails = true;     
    options.Authority = builder.Configuration["Auth0:Authority"];
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

// builder.Services.AddAuth0WebAppAuthentication(options =>
// {
//     options.Domain = builder.Configuration["Auth0:Domain"] ?? throw new InvalidOperationException("Auth0:Domain is required.");
//     options.ClientId = builder.Configuration["Auth0:ClientId"] ?? throw new InvalidOperationException("Auth0:ClientId is required.");
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwaggerUi(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
// app.MapGroup("/identity").MapIdentityApi<ApplicationUser>();
app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.UseExceptionHandler(options =>
{
});

app.Map("/", () => Results.Redirect("/api"));

app.MapEndpoints();

app.Run();

public partial class Program { }
