using System;
using SharpDomain.Core;

namespace VotingSystem.Core.Events
{
    public class AnswerResultIncremented : EventBase
    {
        public AnswerResultIncremented(Guid answerId, Guid questionId)
        {
            AnswerId = answerId;
            QuestionId = questionId;
        }
        
        public Guid AnswerId { get; }
        public Guid QuestionId { get; }
    }
}