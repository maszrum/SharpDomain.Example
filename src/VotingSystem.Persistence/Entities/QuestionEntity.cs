using System;

namespace VotingSystem.Persistence.Entities
{
    public class QuestionEntity
    {
        public Guid Id { get; init; }
        public string QuestionText { get; init; } = string.Empty;

        public override bool Equals(object obj) => 
            obj is QuestionEntity other && Equals(other);

        private bool Equals(QuestionEntity other) => 
            Id.Equals(other.Id);

        public override int GetHashCode() => 
            Id.GetHashCode();
    }
}