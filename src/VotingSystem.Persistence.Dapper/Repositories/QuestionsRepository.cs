using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using VotingSystem.Core.Answer;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Question;
using VotingSystem.Persistence.Dapper.AutoTransaction;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.RepositoryInterfaces;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.Dapper.Repositories
{
    internal class QuestionsRepository : IQuestionsRepository, IQuestionsWriteRepository
    {
        private readonly NpgsqlConnection _connection;
        private readonly ITransactionProvider _transactionProvider;
        private readonly SchemaProvider _schemaProvider;

        private string Schema => _schemaProvider();
        
        public QuestionsRepository(
            NpgsqlConnection connection, 
            ITransactionProvider transactionProvider, 
            SchemaProvider schemaProvider)
        {
            _connection = connection;
            _transactionProvider = transactionProvider;
            _schemaProvider = schemaProvider;
        }

        public async Task<Question?> Get(Guid questionId)
        {
            var sqlQuery = new StringBuilder()
                .AppendLine("SELECT * FROM @Schema.question WHERE id = @Id;")
                .AppendLine("SELECT * FROM @Schema.answer WHERE question_id = @Id;")
                .ToString()
                .InjectSchema(Schema);
            
            var multiQuery = await _connection.QueryMultipleAsync(
                sql: sqlQuery,
                param: new { Id = questionId }, 
                transaction: _transactionProvider.Get());
            
            using (multiQuery)
            {
                var questionEntity = await multiQuery.ReadSingleOrDefaultAsync<QuestionEntity>();
                if (questionEntity is null)
                {
                    return default;
                }
                
                var answerEntities = await multiQuery.ReadAsync<AnswerEntity>();
                
                var answers = answerEntities
                    .Select(e => new Answer(
                        id: e.Id, 
                        questionId: e.QuestionId, 
                        answerOrder: e.AnswerOrder, 
                        text: e.Text, 
                        votes: e.Votes))
                    .ToList();
            
                return new Question(
                    id: questionEntity.Id,
                    questionText: questionEntity.QuestionText,
                    answers: answers);
            }
        }

        public async Task<IReadOnlyList<Question>> GetAll()
        {
            var sqlQuery = new StringBuilder()
                .AppendLine("SELECT * FROM @Schema.question")
                .AppendLine("LEFT OUTER JOIN public.answer ON question.id = answer.question_id")
                .AppendLine("ORDER BY question.id ASC, answer.answer_order ASC")
                .ToString()
                .InjectSchema(Schema);
            
            var entities = new Dictionary<QuestionEntity, List<AnswerEntity>>();
            
            _ = await _connection.QueryAsync<QuestionEntity, AnswerEntity, bool>(
                sql: sqlQuery,
                map: (q, a) =>
                {
                    if (entities.ContainsKey(q))
                    {
                        entities[q].Add(a);
                    }
                    else
                    {
                        entities.Add(q, new List<AnswerEntity> { a });
                    }
                    return true;
                },
                transaction: _transactionProvider.Get());
            
            return entities
                .Select(kvp =>
                {
                    var (questionEntity, answerEntities) = kvp;
                    
                    var answers = answerEntities
                        .Where(a => a is not null)
                        .Select(a => 
                            new Answer(
                                id: a.Id, 
                                questionId: a.QuestionId, 
                                answerOrder: a.AnswerOrder, 
                                text: a.Text, 
                                votes: a.Votes));
                    
                    return new Question(
                        id: questionEntity.Id, 
                        questionText: questionEntity.QuestionText,
                        answers: answers);
                })
                .ToArray();
        }

        public Task<int> GetCount()
        {
            return _connection.ExecuteScalarAsync<int>(
                sql: "SELECT COUNT(*) FROM @Schema.question"
                    .InjectSchema(Schema),
                param: _transactionProvider.Get());
        }

        public Task Create(QuestionEntity question)
        {
            return _connection.ExecuteAsync(
                sql: "INSERT INTO @Schema.question(id, question_text) VALUES (@Id, @QuestionText)"
                    .InjectSchema(Schema),
                param: question,
                transaction: _transactionProvider.Get());
        }
    }
}