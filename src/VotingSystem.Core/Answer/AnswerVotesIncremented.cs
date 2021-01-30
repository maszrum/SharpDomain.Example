using System;
using SharpDomain.Core;

namespace VotingSystem.Core.Answer
{
    public class AnswerVotesIncremented : EventBase
    {
        public AnswerVotesIncremented(Guid answerId, Guid questionId)
        {
            AnswerId = answerId;
            QuestionId = questionId;
        }
        
        public Guid AnswerId { get; }
        public Guid QuestionId { get; }
    }
}