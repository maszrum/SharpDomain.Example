using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Vote;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.RepositoryInterfaces;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.Dapper.Repositories
{
    internal class VotesRepository : IVotesRepository, IVotesWriteRepository
    {
        private readonly NpgsqlConnection _connection;
    
        public VotesRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public Task<IReadOnlyList<Vote>> GetByVoter(Guid voterId)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task Create(VoteEntity vote)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}