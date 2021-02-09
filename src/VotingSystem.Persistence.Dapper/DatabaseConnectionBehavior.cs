using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Npgsql;

namespace VotingSystem.Persistence.Dapper
{
    internal class DatabaseConnectionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : class
    {
        private readonly NpgsqlConnection _npgsqlConnection;

        public DatabaseConnectionBehavior(NpgsqlConnection npgsqlConnection)
        {
            _npgsqlConnection = npgsqlConnection;
        }

        public async Task<TResponse> Handle(
            TRequest request, 
            CancellationToken cancellationToken, 
            RequestHandlerDelegate<TResponse> next)
        {
            await _npgsqlConnection.OpenAsync(cancellationToken);
            
            try
            {
                return await next();
            }
            finally
            {
                await _npgsqlConnection.CloseAsync();
            }
        }
    }
}