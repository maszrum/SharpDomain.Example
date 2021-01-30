using System;
using Autofac;
using MediatR;
using SharpDomain.Application;
using VotingSystem.Application.Identity;
using VotingSystem.Application.Question;
using VotingSystem.Application.Voter;

namespace VotingSystem.ConsoleApp
{
    public static class SeedExtension
    {
        public static ContainerBuilder SeedOnBuild(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterBuildCallback(container =>
            {
                using var scope = container.BeginLifetimeScope();
                var mediator = scope.Resolve<IMediator>();
                var authenticationService = scope.Resolve<AuthenticationService>();

                var createVoter = new CreateVoter("12312312312");
                var voter = mediator.Send(createVoter).GetAwaiter().GetResult()
                    .OnError(error => throw new InvalidOperationException($"cannot create voter while seeding: {error}"));

                var identity = new VoterIdentity(voter.Id, voter.Pesel, voter.IsAdministrator);
                authenticationService.SetIdentity(identity);

                var createQuestion = new CreateQuestion("Some question?", new[] { "Answer 1", "Answer 2", "Answer 3" });
                mediator.Send(createQuestion).GetAwaiter().GetResult()
                    .OnError(error => throw new InvalidOperationException($"cannot create question while seeding: {error}"));
            });

            return containerBuilder;
        }
    }
}
