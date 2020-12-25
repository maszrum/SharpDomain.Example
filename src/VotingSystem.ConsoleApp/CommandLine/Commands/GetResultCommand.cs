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
    internal class GetResultCommand : AuthenticatedCommand
    {
        private readonly IMediator _mediator;
        private readonly AnswerResultChangedNotificator _notificator;

        public GetResultCommand(
            IMediator mediator,
            AnswerResultChangedNotificator notificator,
            AuthenticationService authenticationService,
            ConsoleState consoleState) 
            : base(authenticationService, consoleState)
        {
            _mediator = mediator;
            _notificator = notificator;
        }

        public override async Task Execute(IReadOnlyList<string> args)
        {
            var selectedQuestion = await CommandLineHelper.AskToSelectQuestion(_mediator);
            if (selectedQuestion is null)
            {
                return;
            }

            var getQuestionResult = new GetQuestionResult(selectedQuestion.Id);
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
            var allVotes = questionResult.Answers.Sum(ar => ar.Votes);

            Console.WriteLine();
            Console.WriteLine($"{question.QuestionText}");
            var index = 1;
            foreach (var answerResult in questionResult.Answers)
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

            var votesMutable = questionResult.Answers
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

        public override string GetDefinition() => "get-result";

        private static string GetPercentage(int votes, int allVotes)
        {
            var percentage = 100.0 * votes / allVotes;
            return string.Concat(percentage.ToString("0.00", CultureInfo.InvariantCulture), "%");
        }
    }
}