using System.Threading.Tasks;
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

        public DatabaseInitializer(
            NpgsqlConnection connection, 
            ITransactionProvider transactionProvider, 
            SchemaProvider schemaProvider)
        {
            _connection = connection;
            _transactionProvider = transactionProvider;
            _schemaProvider = schemaProvider;
        }

        protected override Task InitializeIfNeed()
        {
            // TODO: create tables if does not exist
            return Task.CompletedTask;
        }

        protected override Task InitializeForcefully()
        {
            // TODO: remove tables and create
            return Task.CompletedTask;
        }
    }
}