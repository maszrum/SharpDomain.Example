using System;
using SharpDomain.Core;

namespace VotingSystem.Core.Events
{
    public class AnswerResultIncremented : EventBase
    {
        public AnswerResultIncremented(Guid answerResultId, Guid answerId)
        {
            AnswerResultId = answerResultId;
            AnswerId = answerId;
        }
        
        public Guid AnswerResultId { get; }
        public Guid AnswerId { get; }
    }
}