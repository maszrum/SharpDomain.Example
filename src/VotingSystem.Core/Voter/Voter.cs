using System;
using System.Collections.Generic;
using System.Linq;
using SharpDomain.Core;

namespace VotingSystem.Core.Voter
{
    public class Voter : AggregateRoot
    {
        public Voter(
            Guid id, 
            Pesel pesel,
            bool isAdministrator,
            List<Vote.Vote> votes)
        {
            Id = id;
            Pesel = pesel;
            IsAdministrator = isAdministrator;
            _votes = votes;
        }

        private readonly List<Vote.Vote> _votes;
        
        public override Guid Id { get; }
        
        public Pesel Pesel { get; }
        
        public bool IsAdministrator { get; set; }
        
        public IReadOnlyList<Vote.Vote> Votes => _votes;
        
        public Vote.Vote Vote(Guid voterId, Guid questionId, Guid answerId)
        {
            var alreadyVoted = Votes.Any(v => v.QuestionId == questionId);
            if (alreadyVoted)
            {
                throw new TwiceVoteAttemptException();
            }
            
            var voteId = Guid.NewGuid();
            var vote = new Vote.Vote(voteId, voterId, questionId);
            
            _votes.Add(vote);
            
            var votedEvent = new VotePosted(voteId, questionId, answerId);
            Events.Add(votedEvent);
            
            return vote;
        }
        
        public static Voter Create(string? pesel)
        {
            var voterId = Guid.NewGuid();
            var peselValue = new Pesel(pesel);
            
            var voter = new Voter(
                id :voterId, 
                pesel: peselValue, 
                isAdministrator: false, 
                votes: new List<Vote.Vote>());
            
            var createdEvent = new VoterCreated(voterId);
            voter.Events.Add(createdEvent);
            
            return voter;
        }
    }
}