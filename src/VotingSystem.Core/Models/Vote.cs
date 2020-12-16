using System;
using SharpDomain.Core;
using VotingSystem.Core.Events;

namespace VotingSystem.Core.Models
{
    public class Vote : AggregateRoot
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
        
        public static Vote Create(Guid voterId, Guid questionId, Guid answerId)
        {
            var voteId = Guid.NewGuid();
            var vote = new Vote(voteId, voterId, questionId);
            
            var createdEvent = new VotePosted(voteId, questionId, answerId);
            vote.Events.Add(createdEvent);
            
            return vote;
        }
    }
}