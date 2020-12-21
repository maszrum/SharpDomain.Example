using System;
using SharpDomain.Application;

namespace VotingSystem.Application.Commands
{
    public class VoteFor : ICommand
    {
        public VoteFor(Guid voterId, Guid questionId, Guid answerId)
        {
            VoterId = voterId;
            QuestionId = questionId;
            AnswerId = answerId;
        }

        public Guid VoterId { get; }
        public Guid QuestionId { get; }
        public Guid AnswerId { get; }
    }
}