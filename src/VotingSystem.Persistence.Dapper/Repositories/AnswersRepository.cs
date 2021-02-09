using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
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
        private readonly SchemaProvider _schemaProvider;

        private string Schema => _schemaProvider();

        public AnswersRepository(
            NpgsqlConnection connection, 
            ITransactionProvider transactionProvider, 
            SchemaProvider schemaProvider)
        {
            _connection = connection;
            _transactionProvider = transactionProvider;
            _schemaProvider = schemaProvider;
        }

        public async Task<Answer?> Get(Guid answerId)
        {
            var answerEntity = await _connection.QuerySingleOrDefaultAsync<AnswerEntity>(
                sql: "SELECT * FROM @Schema.answer WHERE id = @Id"
                    .InjectSchema(Schema),
                param: new { Id = answerId },
                transaction: _transactionProvider.Get());
            
            return answerEntity is null 
                ? default 
                : new Answer(
                    id: answerEntity.Id, 
                    questionId: answerEntity.QuestionId, 
                    answerOrder: answerEntity.AnswerOrder, 
                    text: answerEntity.Text, 
                    votes: answerEntity.Votes);
        }
        
        public Task Create(params AnswerEntity[] answers)
        {
            return _connection.ExecuteAsync(
                sql: "INSERT INTO @Schema.answer(id, question_id, answer_order, text, votes) VALUES (@Id, @QuestionId, @AnswerOrder, @Text, @Votes)"
                    .InjectSchema(Schema),
                param: answers,
                transaction: _transactionProvider.Get());
        }

        public Task Update(AnswerEntity answer, IReadOnlyList<string> changedProperties)
        {
            var propertiesAssignments = changedProperties
                .Select(p => $"{p.PascalCaseToSnakeCase()} = @{p}");
            
            var assignmentsJoined = string.Join(", ", propertiesAssignments);
                
            return _connection.ExecuteAsync(
                sql: $"UPDATE @Schema.answer SET {assignmentsJoined} WHERE id = @Id"
                    .InjectSchema(Schema),
                param: answer,
                transaction: _transactionProvider.Get());
        }
    }
}