using System.Collections.Generic;
using System.Threading.Tasks;

namespace VotingSystem.ConsoleApp.CommandLine
{
    internal interface IConsoleCommand
    {
        Task Execute(IReadOnlyList<string> args);
        string GetDefinition();
    }
}