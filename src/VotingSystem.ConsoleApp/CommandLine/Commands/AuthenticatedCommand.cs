using System.Collections.Generic;
using System.Threading.Tasks;

namespace VotingSystem.ConsoleApp.CommandLine.Commands
{
    internal abstract class AuthenticatedCommand : IConsoleCommand
    {
        public AuthenticatedCommand(
            AuthenticationService authenticationService,
            ConsoleState consoleState)
        {
            if (consoleState.Identity is not null)
            {
                authenticationService.SetIdentity(consoleState.Identity);
            }
        }

        public abstract Task Execute(IReadOnlyList<string> args);

        public abstract string GetDefinition();
    }
}
