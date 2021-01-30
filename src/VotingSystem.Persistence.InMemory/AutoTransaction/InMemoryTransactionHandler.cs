﻿using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SharpDomain.AutoTransaction;
using VotingSystem.Persistence.InMemory.Datastore;

namespace VotingSystem.Persistence.InMemory.AutoTransaction
{
    internal class InMemoryTransactionHandler<TRequest, TResponse> : TransactionHandler<TRequest, TResponse> 
        where TRequest : notnull
    {
        public InMemoryTransactionHandler(InMemoryDatastore datastore)
        {
            _datastore = datastore;
        }
        
        private readonly InMemoryDatastore _datastore;
        
        public override async Task<TResponse> Handle(
            TRequest request, 
            CancellationToken cancellationToken, 
            RequestHandlerDelegate<TResponse> next)
        {
            Exception? exceptionThrown = default;
            TResponse response = default;
            
            var transaction = await _datastore.BeginTransaction();
            await using (transaction)
            {
                var rolledBack = false;
                try
                {
                    response = await next();
                }
                catch (Exception exception)
                {
                    if (IsRollingBackException(exception))
                    {
                        await transaction.Rollback();
                        rolledBack = true;
                    }
                    
                    exceptionThrown = exception;
                }
                
                if (!rolledBack)
                {
                    await transaction.Commit();
                }
            }
            
            if (exceptionThrown != default)
            {
                ExceptionDispatchInfo.Capture(exceptionThrown).Throw();
            }
            
            return response!;
        }
    }
}