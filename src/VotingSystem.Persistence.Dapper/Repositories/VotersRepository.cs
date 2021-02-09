using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Vote;
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
        private readonly SchemaProvider _schemaProvider;

        private string Schema => _schemaProvider();

        public VotersRepository(
            NpgsqlConnection connection, 
            ITransactionProvider transactionProvider, 
            SchemaProvider schemaProvider)
        {
            _connection = connection;
            _transactionProvider = transactionProvider;
            _schemaProvider = schemaProvider;
        }

        public async Task<Voter?> Get(Guid voterId)
        {
            var sqlQuery = new StringBuilder()
                .AppendLine("SELECT * FROM @Schema.voter WHERE id = @Id;")
                .AppendLine("SELECT * FROM @Schema.vote WHERE voter_id = @Id;")
                .ToString()
                .InjectSchema(Schema);
            
            var multiQuery = await _connection.QueryMultipleAsync(
                sql: sqlQuery,
                param: new { Id = voterId }, 
                transaction: _transactionProvider.Get());
            
            return await ReadVoterFromMultiQuery(multiQuery);
        }

        public async Task<Voter?> GetByPesel(Pesel pesel)
        {
            var sqlQuery = new StringBuilder()
                .AppendLine("SELECT * FROM @Schema.voter WHERE voter.pesel = @Pesel;")
                .AppendLine("SELECT * FROM @Schema.vote WHERE vote.voter_id = (SELECT voter.id FROM @Schema.voter WHERE voter.pesel = @Pesel);")
                .ToString()
                .InjectSchema(Schema);
            
            var multiQuery = await _connection.QueryMultipleAsync(
                sql: sqlQuery,
                param: new { Pesel = pesel.Code }, 
                transaction: _transactionProvider.Get());
            
            return await ReadVoterFromMultiQuery(multiQuery);
        }
        
        private static async Task<Voter?> ReadVoterFromMultiQuery(SqlMapper.GridReader multiQuery)
        {
            using (multiQuery)
            {
                var voterEntity = await multiQuery.ReadSingleOrDefaultAsync<VoterEntity>();
                if (voterEntity is null)
                {
                    return default;
                }
                
                var voteEntities = await multiQuery.ReadAsync<VoteEntity>();
                
                var votes = voteEntities
                    .Select(e => new Vote(e.Id, e.VoterId, e.QuestionId))
                    .ToList();
            
                return new Voter(
                    id: voterEntity.Id, 
                    pesel: new Pesel(voterEntity.Pesel), 
                    isAdministrator: voterEntity.IsAdministrator, 
                    votes: votes);
            }
        }

        public Task<bool> Exists(string pesel)
        {
            return _connection.ExecuteScalarAsync<bool>(
                sql: "SELECT EXISTS(SELECT 1 FROM @Schema.voter WHERE pesel = @Pesel)"
                    .InjectSchema(Schema), 
                param: new { pesel }, 
                transaction: _transactionProvider.Get());
        }

        public Task<int> GetCount()
        {
            return _connection.ExecuteScalarAsync<int>(
                sql: "SELECT COUNT(*) FROM @Schema.voter"
                    .InjectSchema(Schema),
                param: _transactionProvider.Get());
        }

        public Task Create(VoterEntity voter)
        {
            return _connection.ExecuteAsync(
                sql: "INSERT INTO @Schema.voter(id, pesel, is_administrator) VALUES (@Id, @Pesel, @IsAdministrator)"
                    .InjectSchema(Schema), 
                param: voter, 
                transaction: _transactionProvider.Get());
        }

        public Task Update(VoterEntity voter, IReadOnlyList<string> changedProperties)
        {
            var propertiesAssignments = changedProperties
                .Select(p => $"{p.PascalCaseToSnakeCase()} = @{p}");
            
            var assignmentsJoined = string.Join(", ", propertiesAssignments);
                
            return _connection.ExecuteAsync(
                sql: $"UPDATE @Schema.voter SET {assignmentsJoined} WHERE id = @Id"
                    .InjectSchema(Schema),
                param: voter,
                transaction: _transactionProvider.Get());
        }
    }
}