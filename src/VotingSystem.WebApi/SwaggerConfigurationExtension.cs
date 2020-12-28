using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VotingSystem.WebApi
{
    internal static class SwaggerConfigurationExtension
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c => c.AddApiInfo().AddJwtScheme());
        }

        private static SwaggerGenOptions AddApiInfo(this SwaggerGenOptions options)
        {
            var apiInfo = new OpenApiInfo
            {
                Title = "VotingSystem.WebApi",
                Version = "v1"
            };
            options.SwaggerDoc("v1", apiInfo);

            return options;
        } 

        private static SwaggerGenOptions AddJwtScheme(this SwaggerGenOptions options)
        {
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            var requirement = new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                };
            options.AddSecurityRequirement(requirement);

            return options;
        }
    }
}
