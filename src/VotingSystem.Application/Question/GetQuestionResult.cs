using System;
using SharpDomain.Application;
using VotingSystem.Application.Question.ViewModels;

// ReSharper disable once ClassNeverInstantiated.Global

namespace VotingSystem.Application.Question
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