using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Vote;
using VotingSystem.Persistence.Dapper.AutoTransaction;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.RepositoryInterfaces;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.Dapper.Repositories
{
    internal class VotesRepository : IVotesRepository, IVotesWriteRepository
    {
        private readonly NpgsqlConnection _connection;
        private readonly ITransactionProvider _transactionProvider;
        private readonly SchemaProvider _schemaProvider;
    
        private string Schema => _schemaProvider();
        
        public VotesRepository(
            NpgsqlConnection connection, 
            ITransactionProvider transactionProvider, 
            SchemaProvider schemaProvider)
        {
            _connection = connection;
            _transactionProvider = transactionProvider;
            _schemaProvider = schemaProvider;
        }

        public async Task<IReadOnlyList<Vote>> GetByVoter(Guid voterId)
        {
            var voteEntities = await _connection.QueryAsync<VoteEntity>(
                sql: "SELECT * FROM @Schema.vote WHERE voter_id = @VoterId"
                    .InjectSchema(Schema),
                param: new { VoterId = voterId },
                transaction: _transactionProvider.Get());
            
            return voteEntities
                .Select(e => new Vote(
                    id: e.Id, 
                    voterId: e.VoterId, 
                    questionId: e.QuestionId))
                .ToArray();
        }

        public Task Create(VoteEntity vote)
        {
            return _connection.ExecuteAsync(
                sql: "INSERT INTO @Schema.vote(id, voter_id, question_id) VALUES (@Id, @VoterId, @QuestionId)"
                    .InjectSchema(Schema), 
                param: vote, 
                transaction: _transactionProvider.Get());
        }
    }
}