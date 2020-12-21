using System;
using SharpDomain.Application;
using VotingSystem.Application.ViewModels;

namespace VotingSystem.Application.Queries
{
    public class GetMyVotes : IQuery<MyVotesViewModel>
    {
        public GetMyVotes(Guid voterId)
        {
            VoterId = voterId;
        }

        public Guid VoterId { get; }
    }
}