using System;
using SharpDomain.Core;

namespace VotingSystem.Core.Voter
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