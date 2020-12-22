#pragma warning disable 8618

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace VotingSystem.WebApi.Authentication
{
    internal class JwtConfiguration
    {
        public const string Section = "Jwt";
        
        public string Secret { get; init; }
        public string ValidIssuer { get; init; }
        public string ValidAudience { get; init; }
        public int ExpiresInMinutes { get; init; }
    }
}