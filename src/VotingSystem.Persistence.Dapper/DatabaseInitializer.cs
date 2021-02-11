using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using SharpDomain.IoC;
using VotingSystem.Persistence.Dapper.AutoTransaction;

namespace VotingSystem.Persistence.Dapper
{
    internal class DatabaseInitializer : SystemInitializer
    {
        private readonly NpgsqlConnection _connection;
        private readonly ITransactionProvider _transactionProvider;
        private readonly SchemaProvider _schemaProvider;
        
        private string Schema => _schemaProvider();

        public DatabaseInitializer(
            NpgsqlConnection connection, 
            ITransactionProvider transactionProvider, 
            SchemaProvider schemaProvider)
        {
            _connection = connection;
            _transactionProvider = transactionProvider;
            _schemaProvider = schemaProvider;
        }

        protected override async Task InitializeIfNeed()
        {
            var existingTables = await GetExistingTables();
            
            var sqlSource = new TablesSqlSource();
            var expectedTables = sqlSource.GetAvailableTableNames().ToArray();

            foreach (var expectedTable in expectedTables)
            {
                if (!existingTables.Contains(expectedTable))
                {
                    var query = await sqlSource.ReadSqlForTable(expectedTable);
                    
                    _ = await _connection.ExecuteAsync(
                        sql: query.InjectSchema(Schema),
                        param: _transactionProvider.Get());
                }
            }
        }

        protected override async Task InitializeForcefully()
        {
            var existingTables = await GetExistingTables();
            
            var sqlSource = new TablesSqlSource();
            var expectedTables = sqlSource.GetAvailableTableNames().ToArray();

            // drop all tables in reverse order
            foreach (var expectedTable in expectedTables.Reverse())
            {
                if (existingTables.Contains(expectedTable))
                {
                    await DropTable(expectedTable);
                }
            }
            
            // create all tables
            foreach (var expectedTable in expectedTables)
            {
                var query = await sqlSource.ReadSqlForTable(expectedTable);
                    
                _ = await _connection.ExecuteAsync(
                    sql: query.InjectSchema(Schema),
                    param: _transactionProvider.Get());
            }
        }
        
        private async Task<IReadOnlyList<string>> GetExistingTables()
        {
            var existingTables = await _connection.QueryAsync<string>(
                sql: "SELECT tablename FROM pg_tables WHERE schemaname = '@Schema'"
                    .InjectSchema(Schema),
                param: _transactionProvider.Get());
            
            return existingTables.ToArray();
        }
        
        private Task DropTable(string tableName)
        {
            return _connection.ExecuteAsync(
                sql: $"DROP TABLE {Schema}.{tableName}",
                transaction: _transactionProvider.Get());
        }
    }
}