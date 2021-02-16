using System;
using SharpDomain.Application;
using VotingSystem.Application.Questions.ViewModels;

namespace VotingSystem.Application.Questions
{
    public class GetQuestion : IQuery<QuestionViewModel>
    {
        public GetQuestion(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}