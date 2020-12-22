using VotingSystem.Core.InfrastructureAbstractions;

namespace VotingSystem.WebApi.SharpDomain
{
    public interface IAuthenticationService<TIdentity> : IIdentityService<TIdentity>
        where TIdentity : IIdentity
    {
        void SetIdentity(TIdentity identity);
        string GenerateToken(TIdentity identity);
    }
}