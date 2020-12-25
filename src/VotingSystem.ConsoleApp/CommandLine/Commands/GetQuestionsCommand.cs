using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using SharpDomain.Application;
using VotingSystem.Application.Queries;
using VotingSystem.Application.ViewModels;

namespace VotingSystem.ConsoleApp.CommandLine.Commands
{
    internal class GetQuestionsCommand : AuthenticatedCommand
    {
        private readonly IMediator _mediator;

        public GetQuestionsCommand(
            IMediator mediator, 
            AuthenticationService authenticationService, 
            ConsoleState consoleState) 
            : base(authenticationService, consoleState)
        {
            _mediator = mediator;
        }

        public override async Task Execute(IReadOnlyList<string> args)
        {
            var getQuestions = new GetQuestions();
            
            QuestionsListViewModel getQuestionsResponse;
            try
            {
                getQuestionsResponse = await _mediator.Send(getQuestions)
                    .OnError(error => throw new InvalidOperationException(error.ToString()));
            }
            catch (Exception exception)
            {
                exception.WriteToConsole();
                return;
            }

            Console.WriteLine();
            Console.WriteLine(getQuestionsResponse.ToString());
            Console.WriteLine();
        }

        public override string GetDefinition() => "get-questions";
    }
}