using System;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using SharpDomain.Application;
using VotingSystem.Application.Identity;
using VotingSystem.Application.Questions;
using VotingSystem.Application.Voters;

namespace VotingSystem.ConsoleApp
{
    public static class SeedExtension
    {
        public static async Task Seed(this IContainer container)
        {
            await using var scope = container.BeginLifetimeScope();
            var mediator = scope.Resolve<IMediator>();
            var authenticationService = scope.Resolve<AuthenticationService>();

            var logIn = new LogIn("12312312312");
            var logInResult = await mediator.Send(logIn);
            if (!logInResult.TryGet(out var voter))
            {
                var createVoter = new CreateVoter("12312312312");
                await mediator.Send(createVoter)
                    .OnError(error => throw new InvalidOperationException($"cannot create voter while seeding: {error}"));
                
                logIn = new LogIn("12312312312");
                voter = await mediator.Send(logIn)
                    .OnError(error => throw new InvalidOperationException($"cannot create voter while seeding: {error}"));
            }

            var identity = new VoterIdentity(voter.Id, voter.Pesel, voter.IsAdministrator);
            authenticationService.SetIdentity(identity);

            var getQuestionsCount = new GetQuestionsCount();
            var questionsCount = await mediator.Send(getQuestionsCount)
                .OnError(error => throw new InvalidOperationException($"cannot get questions count while seeding: {error}"));
                
            if (questionsCount.Count == 0)
            {
                var createQuestion = new CreateQuestion("Some question?", new[] { "Answer 1", "Answer 2", "Answer 3" });
                await mediator.Send(createQuestion)
                    .OnError(error => throw new InvalidOperationException($"cannot create question while seeding: {error}"));
            }
        }
    }
}
