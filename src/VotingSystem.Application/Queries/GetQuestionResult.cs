using System;
using SharpDomain.Application;
using VotingSystem.Application.ViewModels;

namespace VotingSystem.Application.Queries
{
    public class GetQuestionResult : IQuery<QuestionResultViewModel>
    {
        public GetQuestionResult(Guid questionId, Guid voterId)
        {
            QuestionId = questionId;
            VoterId = voterId;
        }

        public Guid QuestionId { get; }
        public Guid VoterId { get; }
    }
}