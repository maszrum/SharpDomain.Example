using SharpDomain.Core;

namespace VotingSystem.Core.Voters
{
    internal class InvalidPeselException : DomainException
    {
        public string Pesel { get; }

        public InvalidPeselException(string pesel) : base($"specified pesel number is invalid: {pesel}")
        {
            Pesel = pesel;
        }
    }
}