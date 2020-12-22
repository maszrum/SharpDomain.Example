using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VotingSystem.Application.Identity;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.WebApi.Authentication;
using VotingSystem.WebApi.SharpDomain;

namespace VotingSystem.WebApi.VoterAuthentication
{
    internal static class IocExtensions
    {
        private static class ClaimNames
        {
            public const string Id = "user_id";
            public const string Pesel = "pesel";
            public const string IsAdministrator = "is_administrator";
        }
        
        public static void AddVoterAuthentication(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService<VoterIdentity>, VoterAuthenticationService>();
            services.AddScoped<IIdentityService<VoterIdentity>>(
                serviceProvider => serviceProvider.GetRequiredService<IAuthenticationService<VoterIdentity>>());
            
            services.AddScoped<JwtAuthenticationMiddleware<VoterIdentity>>();
        }
        
        public static void ConfigureJwt(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            
            var claimsProvider = scope.ServiceProvider.GetRequiredService<ClaimsProvider>();
            
            claimsProvider.ConfigureClaimsFactory();
            claimsProvider.ConfigureIdentityFactory();
        }
        
        private static void ConfigureClaimsFactory(this ClaimsProvider claimsProvider)
        {
            claimsProvider.ConfigureClaimsFactory<VoterIdentity>(
                voterIdentity => new[]
                {
                    // id
                    new Claim(ClaimNames.Id, voterIdentity.Id.ToString()),
                    // pesel
                    new Claim(ClaimNames.Pesel, voterIdentity.Pesel.ToString()),
                    // is-administrator
                    new Claim(ClaimNames.IsAdministrator, voterIdentity.IsAdministrator.ToString())
                });
        }
        
        private static void ConfigureIdentityFactory(this ClaimsProvider claimsProvider)
        {
            claimsProvider.ConfigureIdentityFactory(
                claims =>
                {
                    var claimsDictionary = claims.ToDictionary(c => c.Type, c => c.Value);
                    
                    // id
                    var idValue = claimsDictionary.GetValueOrDefault(ClaimNames.Id);
                    var id = idValue != default ? Guid.Parse(idValue) : default;
                    
                    // pesel
                    var peselValue = claimsDictionary.GetValueOrDefault(ClaimNames.Pesel);
                    var pesel = peselValue ?? string.Empty;
                    
                    // is-administrator
                    var isAdministratorValue = claimsDictionary.GetValueOrDefault(ClaimNames.IsAdministrator);
                    var isAdministrator = isAdministratorValue != default ? bool.Parse(isAdministratorValue) : default;
                    
                    return new VoterIdentity(id, pesel, isAdministrator);
                });
        }
    }
}