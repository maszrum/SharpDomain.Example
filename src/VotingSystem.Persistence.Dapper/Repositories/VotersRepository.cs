using System;
using System.Threading.Tasks;
using Npgsql;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Voter;
using VotingSystem.Persistence.Dapper.AutoTransaction;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.RepositoryInterfaces;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.Dapper.Repositories
{
    internal class VotersRepository : IVotersRepository, IVotersWriteRepository
    {
        private readonly NpgsqlConnection _connection;
        private readonly ITransactionProvider _transactionProvider;

        public VotersRepository(
            NpgsqlConnection connection, 
            ITransactionProvider transactionProvider)
        {
            _connection = connection;
            _transactionProvider = transactionProvider;
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