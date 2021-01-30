using System;
using Autofac;
using MediatR;
using VotingSystem.ConsoleApp.CommandLine.Commands;
using VotingSystem.ConsoleApp.CommandLine.ResultTracking;
using VotingSystem.Core.Answer;

namespace VotingSystem.ConsoleApp.CommandLine
{
    internal static class AutofacExtensions
    {
        private static readonly Type[] CommandTypes = new[]
        {
            typeof(AddQuestionCommand),
            typeof(GetQuestionsCommand),
            typeof(GetResultCommand),
            typeof(LogInCommand),
            typeof(LogOutCommand),
            typeof(RegisterCommand),
            typeof(VoteCommand)
        };

        public static ContainerBuilder RegisterClientDependencies(this ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterType<AnswerResultIncrementedHandler>()
                .As<INotificationHandler<AnswerVotesIncremented>>()
                .InstancePerLifetimeScope();

            containerBuilder
                .RegisterType<AnswerResultChangedNotificator>()
                .AsSelf()
                .SingleInstance();

            containerBuilder.RegisterType<ConsoleState>()
                .AsSelf()
                .SingleInstance();

            containerBuilder
                .RegisterTypes(CommandTypes)
                .AsSelf()
                .As<IConsoleCommand>()
                .InstancePerDependency();

            return containerBuilder;
        }
    }
}
