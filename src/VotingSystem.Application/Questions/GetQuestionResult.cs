using System;
using SharpDomain.Application;
using VotingSystem.Application.Questions.ViewModels;

// ReSharper disable once ClassNeverInstantiated.Global

namespace VotingSystem.Application.Questions
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