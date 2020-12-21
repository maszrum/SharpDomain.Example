using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VotingSystem.Persistence.Entities;

namespace VotingSystem.Persistence.InMemory.Datastore
{
    internal class InMemoryDatastore
    {
        public InMemoryDatastore()
        {
            _answers = new EntityDatastore<AnswerEntity>();
            _questions = new EntityDatastore<QuestionEntity>();
            _votes = new EntityDatastore<VoteEntity>();
            _voters = new EntityDatastore<VoterEntity>();
            
            _dataStores = new IEntityDatastore[] 
            {
                _answers,
                _questions,
                _votes,
                _voters
            };
        }
        
        // answers
        private readonly EntityDatastore<AnswerEntity> _answers;
        public IDictionary<Guid, AnswerEntity> Answers => _answers.Models;
        
        // questions
        private readonly EntityDatastore<QuestionEntity> _questions;
        public IDictionary<Guid, QuestionEntity> Questions => _questions.Models;
        
        // votes
        private readonly EntityDatastore<VoteEntity> _votes;
        public IDictionary<Guid, VoteEntity> Votes => _votes.Models;
        
        // voters
        private readonly EntityDatastore<VoterEntity> _voters;
        public IDictionary<Guid, VoterEntity> Voters => _voters.Models;
        
        // all data stores
        private readonly IEntityDatastore[] _dataStores;
        
        public Task<InMemoryTransaction> BeginTransaction()
        {
            OnTransactionBegin();
            
            return Task.FromResult(new InMemoryTransaction(OnCommit, OnRollback, OnTransactionDispose));
        }
        
        private void OnTransactionBegin()
        {
            foreach (var datastore in _dataStores)
            {
                datastore.SetSourceToCopy();
            }
        }
        
        private void OnTransactionDispose()
        {
            foreach (var datastore in _dataStores)
            {
                datastore.SetSourceToOrigin();
            }
        }
            
        private Task OnCommit()
        {
            foreach (var datastore in _dataStores)
            {
                datastore.Commit();
            }
            
            return Task.CompletedTask;
        }
            
        private Task OnRollback()
        {
            foreach (var datastore in _dataStores)
            {
                datastore.Rollback();
            }
            
            return Task.CompletedTask;
        }
    }
}