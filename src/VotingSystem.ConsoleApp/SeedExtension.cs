using Autofac;
using MediatR;
using VotingSystem.Application.Commands;

namespace VotingSystem.ConsoleApp
{
    public static class SeedExtension
    {
        public static ContainerBuilder SeedOnBuild(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterBuildCallback(scope =>
            {
                var mediator = scope.Resolve<IMediator>();

                var createVoter = new CreateVoter("12312312312");
                mediator.Send(createVoter).GetAwaiter().GetResult();

                var createQuestion = new CreateQuestion("Some question?", new[] { "Answer 1", "Answer 2", "Answer 3" });
                mediator.Send(createQuestion).GetAwaiter().GetResult();
            });

            return containerBuilder;
        }
    }
}
