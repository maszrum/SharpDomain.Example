﻿using System;
using System.Collections.Generic;

namespace VotingSystem.Persistence.InMemory.Datastore
{
    internal class EntityDatastore<TEntity> : IEntityDatastore where TEntity : class
    {
        private static class TypeLock<T>
        {
            // ReSharper disable once StaticMemberInGenericType
            public static readonly object Lock = new();
        }
        
        private static readonly Dictionary<Guid, TEntity> DataStore  =
            new Dictionary<Guid, TEntity>();
        
        private DictionaryWithHistory<TEntity>? _models;
        
        public IDictionary<Guid, TEntity> Models => _models ?? (IDictionary<Guid, TEntity>) DataStore;
        
        public void Commit()
        {
            if (_models == default)
            {
                throw new InvalidOperationException(
                    "data source is origin, change to copy before");
            }
            
            lock (TypeLock<TEntity>.Lock)
            {
                foreach (var action in _models.Actions)
                {
                    action(DataStore);
                }
            }
        }
        
        public void Rollback() => 
            _models = new DictionaryWithHistory<TEntity>(DataStore);
        
        public void SetSourceToOrigin()
        {
            _models = default;
        }
        
        public void SetSourceToCopy()
        {
            _models = new DictionaryWithHistory<TEntity>(DataStore);
        }
    }
}