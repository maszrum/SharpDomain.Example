using System;

namespace VotingSystem.Persistence.Entities
{
    public class AnswerEntity
    {
        public Guid Id { get; init; }
        public Guid QuestionId { get; init; }
        public int AnswerOrder { get; init; }
        public string Text { get; init; } = string.Empty;
        public int Votes { get; init; }

        public override bool Equals(object obj) => 
            obj is AnswerEntity other && Equals(other);

        private bool Equals(AnswerEntity other) => 
            Id.Equals(other.Id);

        public override int GetHashCode() => 
            Id.GetHashCode();
    }
}