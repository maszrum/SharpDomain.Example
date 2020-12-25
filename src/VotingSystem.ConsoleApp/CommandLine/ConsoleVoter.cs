using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;

namespace VotingSystem.ConsoleApp.CommandLine
{
    internal class ConsoleVoter
    {
        private readonly IContainer _container;
        private readonly ConsoleState _consoleState;
        private readonly Dictionary<string, Func<ILifetimeScope, IConsoleCommand>> _commandsFactory = new();
        private readonly Dictionary<string, string> _commandDefinitions = new();
        
        public ConsoleVoter(IContainer container)
        {
            _container = container;
            _consoleState = _container.Resolve<ConsoleState>();
            SetupCommands(container);
        }
        
        public void RunBlocking()
        {
            ShowWelcomeMessage();
            
            var command = default(string);

            do
            {
                Console.Write(GetReadLinePrefix());
                if (!TryReadLine(out var line))
                {
                    return;
                }
                
                var parts = line.Split(' ');
                if (parts.Length > 0)
                {
                    command = parts[0].ToLower();
                    var args = parts.Skip(1).ToArray();
                    
                    if (command == "help")
                    {
                        ShowHelp();
                    }
                    else if (_commandsFactory.TryGetValue(command, out var commandFactory))
                    {
                        using var scope = _container.BeginLifetimeScope();
                        var commandHandler = commandFactory(scope);
                        commandHandler.Execute(args).GetAwaiter().GetResult();
                    }
                    else if (!IsQuitCommand(command))
                    {
                        Console.WriteLine("Invalid command. Enter valid command or 'help' / 'q'");
                    }
                }
            }
            while (!IsQuitCommand(command));
        }

        private void SetupCommands(IContainer container)
        {
            static string GetCommandText(IConsoleCommand command) =>
                command.GetDefinition().Split(' ')[0];

            using var scope = container.BeginLifetimeScope();

            var commands = scope.Resolve<IEnumerable<IConsoleCommand>>();

            foreach (var command in commands)
            {
                var commandType = command.GetType();
                var commandText = GetCommandText(command);
                _commandsFactory.Add(commandText, scope => (IConsoleCommand)scope.Resolve(commandType));
                _commandDefinitions.Add(commandText, command.GetDefinition());
            }
        }

        private void ShowWelcomeMessage()
        {
            Console.WriteLine();
            Console.WriteLine("Welcome to voting system.");
            Console.WriteLine("Enter 'q' to quit program.");
            Console.WriteLine();
            
            ShowHelp();
            Console.WriteLine();
        }

        private void ShowHelp()
        {
            Console.WriteLine("Available commands:");
            
            foreach (var commandDefinition in _commandDefinitions.Values)
            {
                Console.WriteLine($"  {commandDefinition}");
            }
        }

        private string GetReadLinePrefix()
        {
            return _consoleState.Identity is null
                ? "<not-logged>: " 
                : $"<{_consoleState.Identity.Pesel}>: ";
        }
        
        private static bool TryReadLine(out string line)
        {
            var tries = 0;
            
            do
            {
                line = Console.ReadLine()?.Trim() ?? string.Empty;
                tries++;
                
                if (tries > 5 && string.IsNullOrEmpty(line))
                {
                    return false;
                }
            }
            while (string.IsNullOrEmpty(line));
            
            return true;
        }
        
        private static bool IsQuitCommand(string? command) => command == "q";
    }
}