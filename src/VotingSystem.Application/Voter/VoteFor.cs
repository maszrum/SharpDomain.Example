using System;
using SharpDomain.Application;

// ReSharper disable once ClassNeverInstantiated.Global

namespace VotingSystem.Application.Voter
{
    public class VoteFor : ICommand
    {
        public VoteFor(Guid questionId, Guid answerId)
        {
            QuestionId = questionId;
            AnswerId = answerId;
        }

        public Guid QuestionId { get; }
        public Guid AnswerId { get; }
    }
}