using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SharpDomain.Application;
using VotingSystem.Application.Queries;
using VotingSystem.Application.ViewModels;
using VotingSystem.ConsoleApp.CommandLine.ResultTracking;

namespace VotingSystem.ConsoleApp.CommandLine.Commands
{
    internal class GetResultCommand : IConsoleCommand
    {
        private readonly ConsoleState _consoleState;
        private readonly IMediator _mediator;
        private readonly AnswerResultChangedNotificator _notificator;

        public GetResultCommand(
            ConsoleState consoleState, 
            IMediator mediator, 
            AnswerResultChangedNotificator notificator)
        {
            _consoleState = consoleState;
            _mediator = mediator;
            _notificator = notificator;
        }

        public Task Execute(IReadOnlyList<string> args)
        {
            if (string.IsNullOrEmpty(_consoleState.VoterPesel))
            {
                Console.WriteLine("Log in before using this command.");
                return Task.CompletedTask;
            }
            
            return GetResult();
        }

        private async Task GetResult()
        {
            var selectedQuestion = await CommandLineHelper.AskToSelectQuestion(_mediator);
            if (selectedQuestion is null)
            {
                return;
            }

            var getQuestionResult = new GetQuestionResult(selectedQuestion.Id, _consoleState.VoterId);
            QuestionResultViewModel getQuestionResultResponse;
            try
            {
                getQuestionResultResponse = await _mediator.Send(getQuestionResult)
                    .OnError(error => throw new InvalidOperationException(error.ToString()));
            }
            catch (Exception exception)
            {
                exception.WriteToConsole();
                return;
            }

            WriteResultToConsole(selectedQuestion, getQuestionResultResponse);

            WatchForChanges(selectedQuestion, getQuestionResultResponse);
        }

        private static void WriteResultToConsole(QuestionViewModel question, QuestionResultViewModel questionResult)
        {
            var allVotes = questionResult.AnswerResults.Sum(ar => ar.Votes);

            Console.WriteLine();
            Console.WriteLine($"{question.QuestionText}");
            var index = 1;
            foreach (var answerResult in questionResult.AnswerResults)
            {
                var answerText = question.Answers
                    .Single(a => a.Id == answerResult.AnswerId)
                    .Text;

                Console.WriteLine($"[{index}]: {answerText} - {GetPercentage(answerResult.Votes, allVotes)} ({answerResult.Votes} votes)");

                index++;
            }

            Console.WriteLine();
        }

        private void WatchForChanges(QuestionViewModel question, QuestionResultViewModel questionResult)
        {
            var answerIds = question.Answers
                .Select(a => a.Id)
                .ToArray();

            var votesMutable = questionResult.AnswerResults
                .ToDictionary(a => a.AnswerId, a => a.Votes);

            var subscription = _notificator.Subscribe(
                filter: changedAnswerId => answerIds.Contains(changedAnswerId),
                onNext: changedAnswerId => AnswerResultChanged(changedAnswerId, question, votesMutable));

            using (subscription)
            {
                Console.ReadKey(true);
                Console.WriteLine();
            }
        }

        private static void AnswerResultChanged(Guid answerId, QuestionViewModel shownQuestion, IDictionary<Guid, int> votesMutable)
        {
            var changedAnswer = shownQuestion.Answers.Single(a => a.Id == answerId);

            var currentVotes = ++votesMutable[changedAnswer.Id];
            var allVotes = votesMutable.Sum(v => v.Value);

            var message = $"\rNew vote for: {changedAnswer.Text}. Now: {GetPercentage(currentVotes, allVotes)} ({currentVotes} votes)"
                .PadRight(Console.WindowWidth);
            Console.Write(message);
        }

        public string GetDefinition() => "get-result";

        private static string GetPercentage(int votes, int allVotes)
        {
            var percentage = 100.0 * votes / allVotes;
            return string.Concat(percentage.ToString("0.00", CultureInfo.InvariantCulture), "%");
        }
    }
}