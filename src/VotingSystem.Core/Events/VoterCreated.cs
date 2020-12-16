using System;
using SharpDomain.Core;

namespace VotingSystem.Core.Events
{
    public class VoterCreated : EventBase
    {
        public VoterCreated(Guid voterId)
        {
            VoterId = voterId;
        }
        
        public Guid VoterId { get; }
    }
}