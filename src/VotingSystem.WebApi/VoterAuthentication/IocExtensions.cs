using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SharpDomain.AccessControl;
using SharpDomain.AccessControl.AspNetCore;
using VotingSystem.Application.Identity;
using VotingSystem.WebApi.Jwt;

namespace VotingSystem.WebApi.VoterAuthentication
{
    internal static class IocExtensions
    {
        public static void AddAuthenticationService(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService<VoterIdentity>, VoterAuthenticationService>();
            services.AddScoped<IIdentityService<VoterIdentity>>(
                serviceProvider => serviceProvider.GetRequiredService<IAuthenticationService<VoterIdentity>>());
            
            services.AddScoped<AuthenticationMiddleware<VoterIdentity>>();
        }
        
        public static void ConfigureJwt(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            
            var claimsProvider = scope.ServiceProvider.GetRequiredService<ClaimsIdentityConverter>();
            
            claimsProvider.ConfigureClaimsConverter();
            claimsProvider.ConfigureIdentityConverter();
        }
    }
}