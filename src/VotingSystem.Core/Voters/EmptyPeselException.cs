using SharpDomain.Core;

namespace VotingSystem.Core.Voters
{
    internal class EmptyPeselException : DomainException
    {
        public EmptyPeselException() : base("pesel number was not provided")
        {
        }
    }
}