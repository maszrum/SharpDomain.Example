using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using SharpDomain.Application;
using VotingSystem.Application.Commands;
using VotingSystem.Application.ViewModels;

namespace VotingSystem.ConsoleApp.CommandLine.Commands
{
    internal class AddQuestionCommand : AuthenticatedCommand
    {
        private readonly IMediator _mediator;

        public AddQuestionCommand(
            IMediator mediator, 
            AuthenticationService authenticationService, 
            ConsoleState consoleState) 
            : base(authenticationService, consoleState)
        {
            _mediator = mediator;
        }

        public override Task Execute(IReadOnlyList<string> args)
        {
            if (!TryParseArgs(args, out string text))
            {
                Console.WriteLine($"Invalid args, use: {GetDefinition()}");
                return Task.CompletedTask;
            }

            var answers = ReadAnswersFromConsole(out var cancelled);
            
            if (cancelled)
            {
                Console.WriteLine("Creating of question cancelled.");
                return Task.CompletedTask;
            }
            
            return AddQuestion(text, answers);
        }

        private static IList<string> ReadAnswersFromConsole(out bool cancelled)
        {
            Console.WriteLine("Type the answers by confirming with an enter key.");
            Console.WriteLine("Type 'f' to finish, 'c' to cancel creating question.");
            
            var index = 1;
            string? answer;
            var result = new List<string>();
            do
            {
                Console.Write($"[{index}]: ");
                answer = Console.ReadLine()?.Trim();
                
                if (!string.IsNullOrEmpty(answer) && answer != "f" && answer != "c")
                {
                    index++;
                    result.Add(answer);
                }
            }
            while (answer != "f" && answer != "c");
            
            cancelled = answer == "c";
            
            return result;
        }
        
        private async Task AddQuestion(string question, IList<string> answers)
        {
            var addQuestion = new CreateQuestion(question, answers);
            
            QuestionViewModel addQuestionResponse;
            try
            {
                addQuestionResponse = await _mediator.Send(addQuestion)
                    .OnError(error => throw new InvalidOperationException(error.ToString()));
            }
            catch (Exception exception)
            {
                exception.WriteToConsole();
                return;
            }

            Console.WriteLine();
            Console.WriteLine(addQuestionResponse);
            Console.WriteLine();
        }

        public override string GetDefinition() => "add-question [text]";
        
        private static bool TryParseArgs(IReadOnlyCollection<string> args, out string text)
        {
            text = string.Empty;
            if (args.Count == 0)
            {
                return false;
            }
            
            text = string.Join(' ', args);
            return true;
        }
    }
}