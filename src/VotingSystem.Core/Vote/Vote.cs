using System;
using SharpDomain.Core;

namespace VotingSystem.Core.Vote
{
    public class Vote : Aggregate
    {
        public Vote(
            Guid id, 
            Guid voterId, 
            Guid questionId)
        {
            Id = id;
            VoterId = voterId;
            QuestionId = questionId;
        }

        public override Guid Id { get; }
        
        public Guid VoterId { get; }

        public Guid QuestionId { get; }
    }
}