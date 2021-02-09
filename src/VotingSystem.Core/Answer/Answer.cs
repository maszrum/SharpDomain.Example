using System;
using SharpDomain.Core;
using SharpDomain.Core.ModelStateTracking;

// ReSharper disable once ClassNeverInstantiated.Global

namespace VotingSystem.Core.Answer
{
    public class Answer : Aggregate
    {
        public Answer(
            Guid id, 
            Guid questionId,
            int answerOrder,
            string text,
            int votes)
        {
            Id = id;
            QuestionId = questionId;
            AnswerOrder = answerOrder;
            Text = text;
            Votes = votes;
        }
        
        public override Guid Id { get; }
        
        public Guid QuestionId { get; }
        
        public int AnswerOrder { get; }
        
        public string Text { get; }
        
        public int Votes { get; private set; }
        
        public void IncrementVotes()
        {
            var changedEvent = this.CaptureChangedEvent(
                model => model.Votes++);
            
            var incrementedEvent = new AnswerVotesIncremented(Id, QuestionId);
            
            Events.Add(
                changedEvent, 
                incrementedEvent);
        }

        public override string ToString() => $"{AnswerOrder}. {Text}";
    }
}