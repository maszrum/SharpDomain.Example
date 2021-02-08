using System;
using System.Threading.Tasks;
using Npgsql;
using VotingSystem.Core.Answer;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.RepositoryInterfaces;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.Dapper.Repositories
{
    internal class AnswersRepository : IAnswersWriteRepository, IAnswersRepository
    {
        private readonly NpgsqlConnection _connection;

        public AnswersRepository(NpgsqlConnection connection)
        {
            _connection = connection;
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