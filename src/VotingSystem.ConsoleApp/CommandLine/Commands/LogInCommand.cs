using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using VotingSystem.Application.Queries;
using VotingSystem.Application.ViewModels;

namespace VotingSystem.ConsoleApp.CommandLine.Commands
{
    internal class LogInCommand : IConsoleCommand
    {
        private readonly ConsoleState _consoleState;
        private readonly IMediator _mediator;

        public LogInCommand(ConsoleState consoleState, IMediator mediator)
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
            
            return LogIn(pesel);
        }
        
        private async Task LogIn(string pesel)
        {
            var logIn = new LogIn(pesel);
            
            VoterViewModel logInResult;
            try
            {
                logInResult = await _mediator.Send(logIn);
            }
            catch (Exception exception)
            {
                exception.WriteToConsole();
                return;
            }
            
            _consoleState.VoterId = logInResult.Id;
            _consoleState.VoterPesel = logInResult.Pesel;
        }

        public string GetDefinition() => "login [pesel]";
        
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