using System;
using Autofac;
using VotingSystem.Application.Identity;
using VotingSystem.Core.InfrastructureAbstractions;

namespace VotingSystem.ConsoleApp
{
    internal static class AuthenticationAutofacExtension
    {
        public static ContainerBuilder RegisterAuthentication(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<AuthenticationService>()
                .AsSelf()
                .As<IIdentityService<VoterIdentity>>()
                .InstancePerLifetimeScope();

            return containerBuilder;
        }
    }

    internal class AuthenticationService : IIdentityService<VoterIdentity>
    {
        private VoterIdentity? _identity;

        public bool IsSignedIn => _identity is not null;

        public VoterIdentity GetIdentity()
        {
            if (_identity is null)
            {
                throw new InvalidOperationException(
                    "voter is not signed in");
            }

            return _identity;
        }

        public void SetIdentity(VoterIdentity identity) => 
            _identity = identity;

        public void ResetIdentity() => 
            _identity = default;
    }
}