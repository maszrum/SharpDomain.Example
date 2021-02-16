using System;
using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using SharpDomain.AccessControl;
using SharpDomain.Application;
using SharpDomain.IoC;
using SharpDomain.NUnit;
using VotingSystem.Application.Identity;
using VotingSystem.Application.Voters;
using VotingSystem.Application.Voters.ViewModels;
using VotingSystem.IoC;

namespace VotingSystem.Application.Tests.Integration.TestBase
{
    public abstract class VotingSystemIntegrationTest : IntegrationTest<VotingSystemBuilder>
    {
        private VoterViewModel? _administrator;
        private VoterViewModel? _voter;

        [SetUp]
        public void ResetVoters()
        {
            _administrator = default;
            _voter = default;
        }

        protected override void ConfigureSystem(VotingSystemBuilder systemBuilder) => 
            systemBuilder
                .WithIdentityService<AuthenticationService, VoterIdentity>()
                .Initialize(InitializationType.Forcefully);
        
        protected Task<VoterViewModel> LogInAsAdministrator()
        {
            async Task<VoterViewModel> CreateAndLogIn()
            {
                var admin = await CreateAdministrator();
                SetIdentity(admin);
                return admin;
            }
            
            if (_administrator is null)
            {
                return CreateAndLogIn();
            }
            
            SetIdentity(_administrator);
            return Task.FromResult(_administrator);
        }
        
        protected Task<VoterViewModel> LogInAsVoter()
        {
            async Task<VoterViewModel> CreateAndLogIn()
            {
                var voter = await CreateVoter();
                SetIdentity(voter);
                return voter;
            }
            
            if (_voter is null)
            {
                return CreateAndLogIn();
            }
            
            SetIdentity(_voter);
            return Task.FromResult(_voter);
        }
        
        protected void LogOut()
        {
            var authenticationService = Container.Resolve<AuthenticationService>();
            authenticationService.ResetIdentity();
        }
        
        private void SetIdentity(VoterViewModel user)
        {
            var identity = new VoterIdentity(user.Id, user.Pesel, user.IsAdministrator);
            
            var authenticationService = Container.Resolve<AuthenticationService>();
            authenticationService.SetIdentity(identity);
        }
        
        private async Task<VoterViewModel> CreateAdministrator()
        {
            const string adminPesel = "71102117282";
            
            // first registered user is admin
            var createAdmin = new CreateVoter(adminPesel);
            await Mediator.Send(createAdmin)
                .OnError(error => throw new Exception($"cannot create admin: {error}"));
            
            var logIn = new LogIn(adminPesel);
            _administrator = await Mediator.Send(logIn)
                .OnError(error => throw new Exception($"cannot create admin: {error}"));
            
            return _administrator;
        }
        
        private async Task<VoterViewModel> CreateVoter()
        {
            if (_administrator is null)
            {
                // first is admin, I don't care
                _ = await CreateAdministrator();
            }
            
            const string voterPesel = "99041878149";
            
            // second is not admin
            var createVoter = new CreateVoter(voterPesel);
            await Mediator.Send(createVoter)
                .OnError(error => throw new Exception($"cannot create voter: {error}"));
            
            var logIn = new LogIn(voterPesel);
            _voter = await Mediator.Send(logIn)
                .OnError(error => throw new Exception($"cannot create voter: {error}"));
            
            return _voter;
        }
    }
}