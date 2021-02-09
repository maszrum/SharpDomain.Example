using Npgsql;

namespace VotingSystem.Persistence.Dapper.AutoTransaction
{
    internal class TransactionWrapper : ITransactionProvider
    {
        private NpgsqlTransaction? _transaction;
        
        public void Set(NpgsqlTransaction transaction) => _transaction = transaction;
        
        public void Unset() => _transaction = default;
        
        public NpgsqlTransaction? Get() => _transaction;
    }
}