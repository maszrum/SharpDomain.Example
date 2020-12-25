using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VotingSystem.ConsoleApp.CommandLine.Commands
{
    internal class LogOutCommand : IConsoleCommand
    {
        private readonly AuthenticationService _authenticationService;
        private readonly ConsoleState _consoleState;

        public LogOutCommand(
            AuthenticationService authenticationService,
            ConsoleState consoleState)
        {
            _authenticationService = authenticationService;
            _consoleState = consoleState;
        }

        public Task Execute(IReadOnlyList<string> args)
        {
            if (_consoleState.Identity is null)
            {
                Console.WriteLine("You are not logged.");
                return Task.CompletedTask;
            }

            _authenticationService.ResetIdentity();
            _consoleState.Identity = default;
            
            return Task.CompletedTask;
        }

        public string GetDefinition() => "logout";
    }
}