using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace VotingSystem.WebApi.Authentication
{
    internal static class IocExtensions
    {
        public static void AddJwtAuthentication(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            var jwtConfiguration = configuration
                .GetSection(JwtConfiguration.Section)
                .Get<JwtConfiguration>();
            
            services
                .AddJwtConfiguration(jwtConfiguration)
                .AddJwtEncryptor()
                .AddClaimsProvider()
                .AddAuthenticationService(jwtConfiguration);
        }
        
        private static IServiceCollection AddJwtConfiguration(
            this IServiceCollection services,
            JwtConfiguration instance)
        {
            return services.AddSingleton(typeof(JwtConfiguration), instance);
        }
        
        private static IServiceCollection AddJwtEncryptor(this IServiceCollection services)
        {
            return services.AddScoped<JwtEncryptor>();
        }
        
        private static IServiceCollection AddClaimsProvider(this IServiceCollection services)
        {
            return services.AddScoped<ClaimsProvider>();
        }
        
        private static IServiceCollection AddAuthenticationService(
            this IServiceCollection services, 
            JwtConfiguration jwtConfiguration)
        {
            var secretBytes = Encoding.UTF8.GetBytes(jwtConfiguration.Secret);
            
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = jwtConfiguration.ValidIssuer,
                        ValidAudience = jwtConfiguration.ValidAudience
                    };
                });
            
            return services;
        }
    }
}