using System;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using NUnit.Framework;
using SharpDomain.Application;
using VotingSystem.Application.Identity;
using VotingSystem.Application.Voter;
using VotingSystem.Application.Voter.ViewModels;

namespace VotingSystem.Application.Tests.Integration.TestBase
{
    public abstract class IntegrationTestBase
    {
        private IContainer? _container;
        private IContainer Container
        {
            get => _container ?? throw new NullReferenceException();
            set => _container = value;
        }
        
        private IMediator? _mediator;
        protected IMediator Mediator
        {
            get => _mediator ?? throw new NullReferenceException();
            private set => _mediator = value;
        }
        
        private VoterViewModel? _administrator;
        private VoterViewModel? _voter;
        
        [SetUp]
        public void InitApplication()
        {
            Container = ApplicationBuilder.Build();
            
            Mediator = Container.Resolve<IMediator>();
            
            _administrator = default;
            _voter = default;
        }
        
        [TearDown]
        protected void DisposeApplication()
        {
            Container.Dispose();
        }
        
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
            // first registered user is admin
            var createAdmin = new CreateVoter("71102117282");
            var adminResponse = await Mediator.Send(createAdmin);
            
            var admin = adminResponse.OnError(
                error => throw new Exception($"cannot create admin: {error}"));
            
            _administrator = admin;
            
            return admin;
        }
        
        private async Task<VoterViewModel> CreateVoter()
        {
            if (_administrator is null)
            {
                // first is admin, I don't care
                _ = await CreateAdministrator();
            }
            
            // second is not admin
            var createVoter = new CreateVoter("99041878149");
            var voterResponse = await Mediator.Send(createVoter);
            
            var voter = voterResponse.OnError(
                error => throw new Exception($"cannot create voter: {error}"));
            
            _voter = voter;
            
            return voter;
        }
    }
}