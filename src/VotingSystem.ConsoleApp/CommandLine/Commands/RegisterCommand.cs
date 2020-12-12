using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using VotingSystem.Application.Commands;
using VotingSystem.Application.ViewModels;

namespace VotingSystem.ConsoleApp.CommandLine.Commands
{
    internal class RegisterCommand : IConsoleCommand
    {
        private readonly ConsoleState _consoleState;
        private readonly IMediator _mediator;

        public RegisterCommand(ConsoleState consoleState, IMediator mediator)
        {
            _consoleState = consoleState;
            _mediator = mediator;
        }

        public Task Execute(IReadOnlyList<string> args)
        {
            if (!string.IsNullOrEmpty(_consoleState.VoterPesel))
            {
                Console.WriteLine("Log out before using this command.");
                return Task.CompletedTask;
            }
            
            if (!TryParseArgs(args, out string pesel))
            {
                Console.WriteLine($"Invalid args, use: {GetDefinition()}");
                return Task.CompletedTask;
            }
            
            return Register(pesel);
        }

        private async Task Register(string pesel)
        {
            var createVoter = new CreateVoter(pesel);
            
            VoterViewModel createVoterResponse;
            try
            {
                createVoterResponse = await _mediator.Send(createVoter);
            }
            catch (Exception exception)
            {
                exception.WriteToConsole();
                return;
            }

            Console.WriteLine();
            Console.WriteLine(createVoterResponse.ToString());
            Console.WriteLine();
            Console.WriteLine("Now you can login.");
        }

        public string GetDefinition() => "register [pesel]";
        
        private static bool TryParseArgs(IReadOnlyList<string> args, out string pesel)
        {
            pesel = string.Empty;
            
            if (args.Count != 1)
            {
                return false;
            }
            
            pesel = args[0];
            return true;
        }
    }
}