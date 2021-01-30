using System;
using System.Collections.Generic;
using System.Linq;
using SharpDomain.Core;
using VotingSystem.Core.Vote;

namespace VotingSystem.Core.Voter
{
    public class VoterModel : AggregateRoot
    {
        public VoterModel(
            Guid id, 
            Pesel pesel,
            bool isAdministrator,
            List<VoteModel> votes)
        {
            Id = id;
            Pesel = pesel;
            IsAdministrator = isAdministrator;
            _votes = votes;
        }

        private readonly List<VoteModel> _votes;
        
        public override Guid Id { get; }
        
        public Pesel Pesel { get; }
        
        public bool IsAdministrator { get; set; }
        
        public IReadOnlyList<VoteModel> Votes => _votes;
        
        public VoteModel Vote(Guid voterId, Guid questionId, Guid answerId)
        {
            var alreadyVoted = Votes.Any(v => v.QuestionId == questionId);
            if (alreadyVoted)
            {
                throw new TwiceVoteAttemptException();
            }
            
            var voteId = Guid.NewGuid();
            var vote = new VoteModel(voteId, voterId, questionId);
            
            _votes.Add(vote);
            
            var votedEvent = new VotePosted(voteId, questionId, answerId);
            Events.Add(votedEvent);
            
            return vote;
        }
        
        public static VoterModel Create(string? pesel)
        {
            var voterId = Guid.NewGuid();
            var peselValue = new Pesel(pesel);
            
            var voter = new VoterModel(
                id :voterId, 
                pesel: peselValue, 
                isAdministrator: false, 
                votes: new List<VoteModel>());
            
            var createdEvent = new VoterCreated(voterId);
            voter.Events.Add(createdEvent);
            
            return voter;
        }
    }
}