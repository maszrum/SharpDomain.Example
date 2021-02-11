using System;
using SharpDomain.Core;

namespace VotingSystem.Core.Voters
{
    public class VotePosted : EventBase
    {
        public VotePosted(Guid voteId, Guid questionId, Guid answerId)
        {
            VoteId = voteId;
            QuestionId = questionId;
            AnswerId = answerId;
        }
        
        public Guid VoteId { get; }
        public Guid QuestionId { get; }
        public Guid AnswerId { get; }
    }
}