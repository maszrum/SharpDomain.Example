using System;
using System.Collections.Generic;
using System.Text;
using VotingSystem.Core.Models;

namespace VotingSystem.Application.Question.ViewModels
{
    public class QuestionResultViewModel
    {
        public Guid QuestionId { get; }
        public List<AnswerResultViewModel> Answers { get; } = new();

        public override string ToString() =>
            new StringBuilder()
                .AppendLine($"# {nameof(QuestionModel)}")
                .AppendLine($"{nameof(QuestionId)}: {QuestionId}")
                .Append(string.Join(Environment.NewLine, Answers))
                .ToString();
    }
    
    public class AnswerResultViewModel
    {
        public AnswerResultViewModel(
            Guid answerId, 
            int votes)
        {
            AnswerId = answerId;
            Votes = votes;
        }
            
        public Guid AnswerId { get; }
        public int Votes { get; }

        public override string ToString() =>
            new StringBuilder()
                .AppendLine($"  {nameof(AnswerId)}: {AnswerId}")
                .Append($"  {nameof(Votes)}: {Votes}")
                .ToString();
    }
}