using Npgsql;

namespace VotingSystem.Persistence.Dapper.AutoTransaction
{
    internal interface ITransactionProvider
    {
        NpgsqlTransaction? Get();
    }
}