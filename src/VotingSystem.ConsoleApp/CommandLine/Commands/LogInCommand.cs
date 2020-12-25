using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using SharpDomain.Application;
using VotingSystem.Application.Identity;
using VotingSystem.Application.Queries;
using VotingSystem.Application.ViewModels;

namespace VotingSystem.ConsoleApp.CommandLine.Commands
{
    internal class LogInCommand : IConsoleCommand
    {
        private readonly IMediator _mediator;
        private readonly AuthenticationService _authenticationService;
        private readonly ConsoleState _consoleState;

        public LogInCommand(
            IMediator mediator, 
            AuthenticationService authenticationService,
            ConsoleState consoleState)
        {
            _mediator = mediator;
            _authenticationService = authenticationService;
            _consoleState = consoleState;
        }

        public Task Execute(IReadOnlyList<string> args)
        {
            if (_consoleState.Identity is not null)
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
                logInResult = await _mediator.Send(logIn)
                    .OnError(error => throw new InvalidOperationException(error.ToString()));
            }
            catch (Exception exception)
            {
                exception.WriteToConsole();
                return;
            }

            var identity = new VoterIdentity(
                logInResult.Id, 
                logInResult.Pesel, 
                logInResult.IsAdministrator);

            _authenticationService.SetIdentity(identity);

            _consoleState.Identity = identity;
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