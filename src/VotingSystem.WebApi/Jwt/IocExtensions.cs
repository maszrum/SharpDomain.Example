using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharpDomain.AccessControl.AspNetCore;

namespace VotingSystem.WebApi.Jwt
{
    internal static class IocExtensions
    {
        public static void AddJwt(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            var jwtConfiguration = configuration
                .GetSection(JwtConfiguration.Section)
                .Get<JwtConfiguration>();
            
            services
                .AddJwtConfiguration(jwtConfiguration)
                .AddJwtEncryptor()
                .AddClaimsIdentityConverter()
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
        
        private static IServiceCollection AddClaimsIdentityConverter(this IServiceCollection services)
        {
            services.AddSingleton<ClaimsIdentityConverter>();
            services.AddSingleton<IClaimsIdentityConverter>(
                serviceProvider => serviceProvider.GetRequiredService<ClaimsIdentityConverter>());

            return services;
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
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidIssuer = jwtConfiguration.ValidIssuer,
                        ValidAudience = jwtConfiguration.ValidAudience
                    };
                });
            
            return services;
        }
    }
}