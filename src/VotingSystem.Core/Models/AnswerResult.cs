﻿using System;
using SharpDomain.Core;
using VotingSystem.Core.Events;

namespace VotingSystem.Core.Models
{
    public class AnswerResult : Aggregate
    {
        public AnswerResult(
            Guid id, 
            Guid questionResultId, 
            Guid answerId, 
            int votes)
        {
            Id = id;
            QuestionResultId = questionResultId;
            AnswerId = answerId;
            Votes = votes;
        }

        public override Guid Id { get; }

        public Guid AnswerId { get; }
        
        public Guid QuestionResultId { get; }
        
        public int Votes { get; private set; }
        
        public void IncrementVotes()
        {
            var changedEvent = this.CaptureChangedEvent(
                model => model.Votes++);
            
            var incrementedEvent = new AnswerResultIncremented(Id, AnswerId);
            
            Events.Add(
                changedEvent, 
                incrementedEvent);
        }
    }
}