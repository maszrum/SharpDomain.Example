using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Npgsql;
using SharpDomain.AutoTransaction;

namespace VotingSystem.Persistence.Dapper.AutoTransaction
{
    internal class PostgresqlTransactionHandler<TRequest, TResponse> : TransactionHandler<TRequest, TResponse> 
        where TRequest : notnull
    {
        private readonly NpgsqlConnection _connection;
        private readonly TransactionWrapper _transactionWrapper;

        public PostgresqlTransactionHandler(
            NpgsqlConnection connection, 
            TransactionWrapper transactionWrapper)
        {
            _connection = connection;
            _transactionWrapper = transactionWrapper;
        }

        public override async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            Exception? exceptionThrown = default;
            TResponse response = default;
            
            await using (var transaction = await _connection.BeginTransactionAsync(cancellationToken))
            {
                _transactionWrapper.Set(transaction);
                
                var rolledBack = false;
                try
                {
                    response = await next();
                }
                catch (Exception exception)
                {
                    if (IsRollingBackException(exception))
                    {
                        await transaction.RollbackAsync(CancellationToken.None);
                        rolledBack = true;
                    }
                    
                    exceptionThrown = exception;
                }
                
                if (!rolledBack)
                {
                    await transaction.CommitAsync(CancellationToken.None);
                }
                
                _transactionWrapper.Unset();
            }
            
            if (exceptionThrown != default)
            {
                ExceptionDispatchInfo.Capture(exceptionThrown).Throw();
            }
            
            return response!;
        }
    }
}