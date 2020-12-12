using System;

namespace VotingSystem.Application.Exceptions
{
    internal class DenialOfAccessToVotingResultsException : ApplicationException
    {
        public Guid VoterId { get; }
        public Guid QuestionId { get; }

        public DenialOfAccessToVotingResultsException(Guid voterId, Guid questionId) 
            : base($"voter has no access to the results of the vote because he/she did not vote")
        {
            VoterId = voterId;
            QuestionId = questionId;
        }
    }
}