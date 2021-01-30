using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VotingSystem.Core.Models;

namespace VotingSystem.Application.Question.ViewModels
{
    public class QuestionsListViewModel
    {
        public List<QuestionViewModel> Questions { get; } = new ();

        public override string ToString()
        {
            var questionsFormatted = Questions
                .SelectMany((q, i) =>
                {
                    var lines = q.ToString().Split(Environment.NewLine);
                    lines[0] = $"[{i}] {lines[0]}";
                    return lines;
                })
                .Select(q => $"  {q}");

            var questionsString = string.Join($"{Environment.NewLine}", questionsFormatted);

            return new StringBuilder()
                .AppendLine($"# {nameof(QuestionModel)}[]")
                .Append(questionsString)
                .ToString();
        }
    }
}