using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Vote;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.InMemory.Datastore;
using VotingSystem.Persistence.InMemory.Exceptions;
using VotingSystem.Persistence.RepositoryInterfaces;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.InMemory.Repositories
{
    internal class VotesRepository : IVotesRepository, IVotesWriteRepository
    {
        private readonly InMemoryDatastore _datastore;
    
        public VotesRepository(InMemoryDatastore datastore)
        {
            _datastore = datastore;
        }

        public Task<IReadOnlyList<VoteModel>> GetByVoter(Guid voterId)
        {
            var votesEntities = _datastore.Votes.Values
                .Where(v => v.VoterId == voterId);
            
            var votes = votesEntities
                .Select(e => new VoteModel(e.Id, e.VoterId, e.QuestionId))
                .ToList();
            
            return Task.FromResult((IReadOnlyList<VoteModel>)votes);
        }

        public Task Create(VoteEntity vote)
        {
            var exists = _datastore.Votes.ContainsKey(vote.Id);
            if (exists)
            {
                throw new EntityAlreadyExistsException(
                    typeof(VoteEntity), vote.Id);
            }
            
            _datastore.Votes.Add(vote.Id, vote);
            
            return Task.CompletedTask;
        }
    }
}