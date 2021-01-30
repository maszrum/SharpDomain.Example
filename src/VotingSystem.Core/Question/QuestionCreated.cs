using System;
using SharpDomain.Core;

namespace VotingSystem.Core.Question
{
    public class QuestionCreated : EventBase
    {
        public QuestionCreated(Guid questionId)
        {
            QuestionId = questionId;
        }
        
        public Guid QuestionId { get; }
    }
}