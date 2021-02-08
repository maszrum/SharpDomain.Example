using System;
using System.Threading.Tasks;
using Npgsql;
using VotingSystem.Core.Answer;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Persistence.Dapper.AutoTransaction;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.RepositoryInterfaces;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.Dapper.Repositories
{
    internal class AnswersRepository : IAnswersWriteRepository, IAnswersRepository
    {
        private readonly NpgsqlConnection _connection;
        private readonly ITransactionProvider _transactionProvider;

        public AnswersRepository(
            NpgsqlConnection connection, 
            ITransactionProvider transactionProvider)
        {
            _connection = connection;
            _transactionProvider = transactionProvider;
        }

        public Task<Answer?> Get(Guid answerId)
        {
            // TODO
            throw new NotImplementedException();
        }
        
        public Task Create(params AnswerEntity[] answers)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task Update(AnswerEntity answer)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}