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
        
        [SetUp]
        public void InitApplication()
        {
            Container = ApplicationBuilder.Build();
            
            Mediator = Container.Resolve<IMediator>();
        }
        
        [TearDown]
        protected void DisposeApplication()
        {
            Container.Dispose();
        }
        
        protected async Task<VoterViewModel> LogInAsAdministrator()
        {
            var admin = await CreateAdministrator();
            SetIdentity(admin);
            
            return admin;
        }
        
        protected async Task<VoterViewModel> LogInAsVoter()
        {
            var voter = await CreateVoter();
            SetIdentity(voter);
            
            return voter;
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
            
            return admin;
        }
        
        private async Task<VoterViewModel> CreateVoter()
        {
            // first is admin, I don't care
            _ = await CreateAdministrator();
            
            // second is not admin
            var createVoter = new CreateVoter("99041878149");
            var voterResponse = await Mediator.Send(createVoter);
            
            var voter = voterResponse.OnError(
                error => throw new Exception($"cannot create voter: {error}"));
            
            return voter;
        }
    }
}