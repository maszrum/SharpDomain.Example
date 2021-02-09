using System;

namespace VotingSystem.Persistence.Entities
{
    public class VoteEntity
    {
        public Guid Id { get; init; }
        public Guid VoterId { get; init; }
        public Guid QuestionId { get; init; }

        public override bool Equals(object obj) => 
            obj is VoteEntity other && Equals(other);

        private bool Equals(VoteEntity other) => 
            Id.Equals(other.Id);

        public override int GetHashCode() => 
            Id.GetHashCode();
    }
}