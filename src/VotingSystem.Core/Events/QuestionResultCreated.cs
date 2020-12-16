using System;
using SharpDomain.Core;

namespace VotingSystem.Core.Events
{
    public class QuestionResultCreated : EventBase
    {
        public QuestionResultCreated(Guid questionResultId, Guid questionId)
        {
            QuestionResultId = questionResultId;
            QuestionId = questionId;
        }

        public Guid QuestionResultId { get; }
        public Guid QuestionId { get; }
    }
}