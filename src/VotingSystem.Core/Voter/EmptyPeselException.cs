using SharpDomain.Core;

namespace VotingSystem.Core.Voter
{
    internal class EmptyPeselException : DomainException
    {
        public EmptyPeselException() : base("pesel number was not provided")
        {
        }
    }
}