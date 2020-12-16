using System;
using SharpDomain.Core;
using VotingSystem.Core.Events;
using VotingSystem.Core.ValueObjects;

namespace VotingSystem.Core.Models
{
    public class Voter : AggregateRoot
    {
        public Voter(
            Guid id, 
            Pesel pesel,
            bool isAdministrator)
        {
            Id = id;
            Pesel = pesel;
            IsAdministrator = isAdministrator;
        }

        public override Guid Id { get; }
        
        public Pesel Pesel { get; }
        
        public bool IsAdministrator { get; set; }
        
        public static Voter Create(string? pesel)
        {
            var voterId = Guid.NewGuid();
            var peselValue = new Pesel(pesel);
            
            var voter = new Voter(voterId, peselValue, isAdministrator: false);
            
            var createdEvent = new VoterCreated(voterId);
            voter.Events.Append(createdEvent);
            
            return voter;
        }
    }
}