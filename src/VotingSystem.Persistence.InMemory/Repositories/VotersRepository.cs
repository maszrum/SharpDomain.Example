using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Votes;
using VotingSystem.Core.Voters;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.InMemory.Datastore;
using VotingSystem.Persistence.InMemory.Exceptions;
using VotingSystem.Persistence.RepositoryInterfaces;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.InMemory.Repositories
{
    internal class VotersRepository : IVotersRepository, IVotersWriteRepository
    {
        private readonly InMemoryDatastore _datastore;

        public VotersRepository(InMemoryDatastore datastore)
        {
            _datastore = datastore;
        }

        public Task<Voter?> Get(Guid voterId)
        {
            if (_datastore.Voters.TryGetValue(voterId, out var entity))
            {
                var votes = _datastore.Votes.Values
                    .Where(e => e.VoterId == voterId)
                    .Select(e => new Vote(e.Id, e.VoterId, e.QuestionId))
                    .ToList();
                
                var pesel = new Pesel(entity.Pesel);
                
                var voter = new Voter(
                    entity.Id, 
                    pesel, 
                    entity.IsAdministrator, 
                    votes);
                
                return Task.FromResult((Voter?)voter);
            }
            
            return Task.FromResult(default(Voter));
        }

        public Task<Voter?> GetByPesel(Pesel pesel)
        {
            var voterEntity = _datastore.Voters.Values
                .SingleOrDefault(e => pesel.Equals(e.Pesel));
            
            return voterEntity is not null 
                ? Get(voterEntity.Id) 
                : Task.FromResult(default(Voter));
        }

        public Task<bool> Exists(string pesel)
        {
            if (pesel == null)
            {
                throw new ArgumentNullException(nameof(pesel));
            }

            var exists = _datastore.Voters.Values.Any(v => v.Pesel == pesel);
            return Task.FromResult(exists);
        }

        public Task<int> GetCount()
        {
            var count = _datastore.Voters.Count;
            return Task.FromResult(count);
        }

        public Task Create(VoterEntity voter)
        {
            if (_datastore.Voters.ContainsKey(voter.Id))
            {
                throw new EntityAlreadyExistsException(
                    typeof(VoterEntity), voter.Id);
            }
            
            _datastore.Voters.Add(voter.Id, voter);
            
            return Task.CompletedTask;
        }

        public Task Update(VoterEntity voter, IReadOnlyList<string> changedProperties)
        {
            if (!_datastore.Voters.ContainsKey(voter.Id))
            {
                throw new EntityNotFoundException(
                    typeof(VoterEntity), voter.Id);
            }
            
            _datastore.Voters[voter.Id] = voter;
            
            return Task.CompletedTask;
        }
    }
}