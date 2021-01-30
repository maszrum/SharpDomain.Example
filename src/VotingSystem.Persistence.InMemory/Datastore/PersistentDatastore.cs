using System;
using System.Collections.Generic;

namespace VotingSystem.Persistence.InMemory.Datastore
{
    internal class PersistentDatastore<TEntity>
    {
        private readonly object _dataLock = new();
        
        public Dictionary<Guid, TEntity> Data { get; } = new();
        
        public void DoWithLock(Action<Dictionary<Guid, TEntity>> action)
        {
            lock (_dataLock)
            {
                action(Data);
            }
        }
    }
}