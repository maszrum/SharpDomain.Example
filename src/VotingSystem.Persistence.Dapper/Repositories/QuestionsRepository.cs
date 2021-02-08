using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Question;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.RepositoryInterfaces;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.Dapper.Repositories
{
    internal class QuestionsRepository : IQuestionsRepository, IQuestionsWriteRepository
    {
        private readonly NpgsqlConnection _connection;

        public QuestionsRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public Task<Question?> Get(Guid questionId)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Question>> GetAll()
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task Create(QuestionEntity question)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}