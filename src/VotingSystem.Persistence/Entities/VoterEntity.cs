using System;

namespace VotingSystem.Persistence.Entities
{
    public class VoterEntity
    {
        public Guid Id { get; init; }
        public string Pesel { get; init; } = string.Empty;
        public bool IsAdministrator { get; init; }

        public override bool Equals(object obj) => 
            obj is VoterEntity other && Equals(other);

        private bool Equals(VoterEntity other) => 
            Id.Equals(other.Id);

        public override int GetHashCode() => 
            Id.GetHashCode();
    }
}