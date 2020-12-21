using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using SharpDomain.Application;
using VotingSystem.Application.Commands;

namespace VotingSystem.ConsoleApp.CommandLine.Commands
{
    internal class VoteCommand : IConsoleCommand
    {
        private readonly ConsoleState _consoleState;
        private readonly IMediator _mediator;

        public VoteCommand(ConsoleState consoleState, IMediator mediator)
        {
            _consoleState = consoleState;
            _mediator = mediator;
        }

        public Task Execute(IReadOnlyList<string> args)
        {
            if (string.IsNullOrEmpty(_consoleState.VoterPesel))
            {
                Console.WriteLine("Log in before using this command.");
                return Task.CompletedTask;
            }
            
            return DoVote();
        }

        private async Task DoVote()
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
                _consoleState.VoterId, 
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

        public string GetDefinition() => "vote";
    }
}