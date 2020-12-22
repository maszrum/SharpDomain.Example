using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.WebApi.Authentication;

namespace VotingSystem.WebApi.SharpDomain
{
    internal class JwtAuthenticationMiddleware<TIdentity> : IMiddleware
        where TIdentity : IIdentity
    {
        private readonly ClaimsProvider _claimsProvider;
        private readonly IAuthenticationService<TIdentity> _authenticationService;

        public JwtAuthenticationMiddleware(
            ClaimsProvider claimsProvider, 
            IAuthenticationService<TIdentity> authenticationService)
        {
            _claimsProvider = claimsProvider;
            _authenticationService = authenticationService;
        }

        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var claims = context.User.Claims;
            
            if (_claimsProvider.TryGetIdentity<TIdentity>(claims, out var identity))
            {
                _authenticationService.SetIdentity(identity);
            }
            
            return next(context);
        }
    }
}