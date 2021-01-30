using System;
using System.Collections.Generic;

namespace VotingSystem.Persistence.InMemory.Datastore
{
    internal class EntityDatastore<TEntity> : IEntityDatastore where TEntity : class
    {
        private readonly PersistentDatastore<TEntity> _persistentDatastore;
        private DictionaryWithHistory<TEntity>? _models;

        public EntityDatastore(PersistentDatastore<TEntity> persistentDatastore)
        {
            _persistentDatastore = persistentDatastore;
        }

        public IDictionary<Guid, TEntity> Models => _models ?? (IDictionary<Guid, TEntity>) _persistentDatastore.Data;
        
        public void Commit()
        {
            if (_models == default)
            {
                throw new InvalidOperationException(
                    "data source is origin, change to copy before");
            }
            
            _persistentDatastore.DoWithLock(ds =>
            {
                foreach (var action in _models.Actions)
                {
                    action(ds);
                }
            });
        }
        
        public void Rollback() => 
            _models = new DictionaryWithHistory<TEntity>(_persistentDatastore.Data);
        
        public void SetSourceToOrigin()
        {
            _models = default;
        }
        
        public void SetSourceToCopy()
        {
            _models = new DictionaryWithHistory<TEntity>(_persistentDatastore.Data);
        }
    }
}