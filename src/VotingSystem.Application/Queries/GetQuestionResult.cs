using System;
using SharpDomain.Application;
using VotingSystem.Application.ViewModels;

namespace VotingSystem.Application.Queries
{
    public class GetQuestionResult : IQuery<QuestionResultViewModel>
    {
        public GetQuestionResult(Guid questionId)
        {
            QuestionId = questionId;
        }

        public Guid QuestionId { get; }
    }
}