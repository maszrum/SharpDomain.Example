using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using SharpDomain.Application;
using VotingSystem.Application.Voters;

namespace VotingSystem.ConsoleApp.CommandLine.Commands
{
    internal class VoteCommand : AuthenticatedCommand
    {
        private readonly IMediator _mediator;

        public VoteCommand(
            IMediator mediator, 
            AuthenticationService authenticationService, 
            ConsoleState consoleState) 
            : base(authenticationService, consoleState)
        {
            _mediator = mediator;
        }

        public override async Task Execute(IReadOnlyList<string> args)
        {
            var selectedQuestion = await CommandLineHelper.AskToSelectQuestion(_mediator);
            if (selectedQuestion is null)
            {
                return;
            }

            var selectedAnswer = CommandLineHelper.AskToSelectAnswer(selectedQuestion);
            if (selectedAnswer is null)
            {
                return;
            }

            var voteFor = new VoteFor(
                selectedQuestion.Id,
                selectedAnswer.Id);

            try
            {
                await _mediator.Send(voteFor)
                    .OnError(error => throw new InvalidOperationException(error.ToString()));
            }
            catch (Exception exception)
            {
                exception.WriteToConsole();
                return;
            }

            Console.WriteLine($"Voted for: {selectedAnswer.Text}");
        }

        public override string GetDefinition() => "vote";
    }
}