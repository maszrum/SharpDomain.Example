using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using VotingSystem.Application.Identity;
using VotingSystem.WebApi.Jwt;

namespace VotingSystem.WebApi.VoterAuthentication
{
    internal static class ClaimsConfigurationExtensions
    {
        private static class ClaimNames
        {
            public const string Id = "user_id";
            public const string Pesel = "pesel";
            public const string IsAdministrator = "is_administrator";
        }

        public static void ConfigureClaimsConverter(this ClaimsIdentityConverter claimsProvider)
        {
            claimsProvider.ConfigureClaimsConverter<VoterIdentity>(
                voterIdentity => new[]
                {
                    // id
                    new Claim(ClaimNames.Id, voterIdentity.Id.ToString()),
                    // pesel
                    new Claim(ClaimNames.Pesel, voterIdentity.Pesel.ToString()),
                    // is administrator
                    new Claim(ClaimNames.IsAdministrator, voterIdentity.IsAdministrator.ToString())
                });
        }

        public static void ConfigureIdentityConverter(this ClaimsIdentityConverter claimsProvider)
        {
            claimsProvider.ConfigureIdentityConverter(
                claims =>
                {
                    var claimsDictionary = claims.ToDictionary(c => c.Type, c => c.Value);

                    // id
                    var idValue = claimsDictionary.GetValueOrDefault(ClaimNames.Id);
                    var id = idValue != default ? Guid.Parse(idValue) : default;

                    // pesel
                    var peselValue = claimsDictionary.GetValueOrDefault(ClaimNames.Pesel);
                    var pesel = peselValue ?? string.Empty;

                    // is administrator
                    var isAdministratorValue = claimsDictionary.GetValueOrDefault(ClaimNames.IsAdministrator);
                    var isAdministrator = !string.IsNullOrEmpty(isAdministratorValue) && bool.Parse(isAdministratorValue);

                    return new VoterIdentity(id, pesel, isAdministrator);
                });
        }
    }
}