using System;
using System.Threading.Tasks;
using Npgsql;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Voter;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.RepositoryInterfaces;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.Dapper.Repositories
{
    internal class VotersRepository : IVotersRepository, IVotersWriteRepository
    {
        private readonly NpgsqlConnection _connection;

        public VotersRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public Task<Voter?> Get(Guid voterId)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task<Voter?> GetByPesel(Pesel pesel)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task<bool> Exists(string pesel)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task<int> GetCount()
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task Create(VoterEntity voter)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task Update(VoterEntity voter)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}