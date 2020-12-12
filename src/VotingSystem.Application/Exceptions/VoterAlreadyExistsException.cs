using System;

namespace VotingSystem.Application.Exceptions
{
    internal class VoterAlreadyExistsException : ApplicationException
    {
        public string Pesel { get; }

        public VoterAlreadyExistsException(string pesel) 
            : base($"voter with pesel {pesel} already exists")
        {
            Pesel = pesel;
        }
    }
}