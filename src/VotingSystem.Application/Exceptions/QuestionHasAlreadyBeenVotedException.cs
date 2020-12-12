using System;

namespace VotingSystem.Application.Exceptions
{
    internal class QuestionHasAlreadyBeenVotedException : ApplicationException
    {
        public Guid QuestionId { get; }
        public Guid VoterId { get; }

        public QuestionHasAlreadyBeenVotedException(Guid questionId, Guid voterId) 
            : base($"this question has already been voted by this voter")
        {
            QuestionId = questionId;
            VoterId = voterId;
        }
    }
}