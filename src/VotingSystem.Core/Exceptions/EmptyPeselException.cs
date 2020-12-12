using SharpDomain.Core;

namespace VotingSystem.Core.Exceptions
{
    internal class EmptyPeselException : DomainException
    {
        public EmptyPeselException() : base("pesel number was not provided")
        {
        }
    }
}