using System.Net.Http.Headers;
using Blog.Application.Common.Interfaces;
using Blog.Domain.Constants;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Data.Interceptors;
using Blog.Infrastructure.Identity;
using Blog.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddTransient<IFileService, CloudinaryFileService>();
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        //
        // services.AddAuthentication()
        //     .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorizationBuilder();

        services.AddSingleton(TimeProvider.System);

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator));
            options.AddPolicy("CanCreateArticle", policy => policy.RequireClaim("permissions", "create:article"));
        });
        services.AddHttpClient("Auth0", (servicesProvider, client) =>
        {
            var configuration = servicesProvider.GetRequiredService<IConfiguration>();
            var apiEndpoint = configuration["Auth0:ManagementApi"] ??
                              throw new InvalidOperationException("Auth0:ManagementApi is required.");
            client.BaseAddress = new Uri(apiEndpoint);
        });
        services.AddTransient<Auth0TokenManager>();
        services.AddTransient<UserService>();
        return services;
    }
}
