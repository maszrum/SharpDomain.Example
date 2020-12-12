using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using VotingSystem.Application.Queries;
using VotingSystem.Application.ViewModels;

namespace VotingSystem.ConsoleApp.CommandLine.Commands
{
    internal class GetQuestionsCommand : IConsoleCommand
    {
        private readonly IMediator _mediator;

        public GetQuestionsCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(IReadOnlyList<string> args)
        {
            var getQuestions = new GetQuestions();
            
            QuestionsListViewModel getQuestionsResponse;
            try
            {
                getQuestionsResponse = await _mediator.Send(getQuestions);
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

        public string GetDefinition() => "get-questions";
    }
}