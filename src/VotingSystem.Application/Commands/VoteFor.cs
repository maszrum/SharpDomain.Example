using System;
using SharpDomain.Application;

namespace VotingSystem.Application.Commands
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