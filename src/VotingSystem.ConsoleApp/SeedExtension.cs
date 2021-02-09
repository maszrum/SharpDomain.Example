using System;
using Autofac;
using MediatR;
using SharpDomain.Application;
using VotingSystem.Application.Identity;
using VotingSystem.Application.Questions;
using VotingSystem.Application.Voters;
using VotingSystem.Application.Voters.ViewModels;

namespace VotingSystem.ConsoleApp
{
    public static class SeedExtension
    {
        // TODO: remove GetAwaiter().GetResult()
        public static ContainerBuilder SeedOnBuild(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterBuildCallback(container =>
            {
                using var scope = container.BeginLifetimeScope();
                var mediator = scope.Resolve<IMediator>();
                var authenticationService = scope.Resolve<AuthenticationService>();

                var logIn = new LogIn("12312312312");
                var logInResult = mediator.Send(logIn).GetAwaiter().GetResult();
                if (!logInResult.TryGet(out var voter))
                {
                    var createVoter = new CreateVoter("12312312312");
                    voter = mediator.Send(createVoter).GetAwaiter().GetResult()
                        .OnError(error => throw new InvalidOperationException($"cannot create voter while seeding: {error}"));
                }
                
                var identity = new VoterIdentity(voter.Id, voter.Pesel, voter.IsAdministrator);
                authenticationService.SetIdentity(identity);

                var getQuestionsCount = new GetQuestionsCount();
                var questionsCount = mediator.Send(getQuestionsCount).GetAwaiter().GetResult()
                    .OnError(error => throw new InvalidOperationException($"cannot get questions count while seeding: {error}"));
                
                if (questionsCount.Count == 0)
                {
                    var createQuestion = new CreateQuestion("Some question?", new[] { "Answer 1", "Answer 2", "Answer 3" });
                    mediator.Send(createQuestion).GetAwaiter().GetResult()
                        .OnError(error => throw new InvalidOperationException($"cannot create question while seeding: {error}"));
                }
            });

            return containerBuilder;
        }
    }
}
