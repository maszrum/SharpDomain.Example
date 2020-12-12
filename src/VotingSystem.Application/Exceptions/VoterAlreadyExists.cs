using System;

namespace VotingSystem.Application.Exceptions
{
    internal class VoterAlreadyExists : ApplicationException
    {
        public string? Pesel { get; }

        public VoterAlreadyExists(string? pesel) 
            : base($"voter with pesel {pesel} already exists")
        {
            Pesel = pesel;
        }
    }
}