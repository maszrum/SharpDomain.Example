using SharpDomain.Core;

namespace VotingSystem.Core.Voter
{
    internal class TwiceVoteAttemptException : DomainException
    {
        public TwiceVoteAttemptException() 
            : base($"only one vote can be cast per question")
        {
        }
    }
}