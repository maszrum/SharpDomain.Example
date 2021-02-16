using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using SharpDomain.Application;
using VotingSystem.Application.Voters;
using VotingSystem.Application.Voters.ViewModels;

namespace VotingSystem.ConsoleApp.CommandLine.Commands
{
    internal class RegisterCommand : IConsoleCommand
    {
        private readonly IMediator _mediator;
        private readonly AuthenticationService _authenticationService;

        public RegisterCommand(
            IMediator mediator, 
            AuthenticationService authenticationService)
        {
            _mediator = mediator;
            _authenticationService = authenticationService;
        }

        public Task Execute(IReadOnlyList<string> args)
        {
            if (_authenticationService.IsSignedIn)
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
            
            VoterViewModel voterViewModel;
            try
            {
                var voterId = await _mediator.Send(createVoter)
                    .OnError(error => throw new InvalidOperationException(error.ToString()));
                
                var logIn = new LogIn(pesel);
                voterViewModel = await _mediator.Send(logIn)
                    .OnError(error => throw new InvalidOperationException(error.ToString()));
            }
            catch (Exception exception)
            {
                exception.WriteToConsole();
                return;
            }

            Console.WriteLine();
            Console.WriteLine(voterViewModel.ToString());
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