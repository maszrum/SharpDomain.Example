using System;
using SharpDomain.AccessControl;

namespace VotingSystem.Application.Identity
{
    public class VoterIdentity : IIdentity
    {
        public Guid Id { get; }
        public string Pesel { get; }
        public bool IsAdministrator { get; }
        
        public VoterIdentity(Guid id, string pesel, bool isAdministrator)
        {
            Id = id;
            Pesel = pesel;
            IsAdministrator = isAdministrator;
        }

        public bool IsValid() => 
            Id != Guid.Empty && !string.IsNullOrEmpty(Pesel);
    }
}