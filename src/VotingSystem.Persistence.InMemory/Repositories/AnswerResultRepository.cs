using System;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Models;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.InMemory.Datastore;
using VotingSystem.Persistence.InMemory.Exceptions;
using VotingSystem.Persistence.RepositoryInterfaces;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.InMemory.Repositories
{
    internal class AnswerResultRepository : IAnswerResultsRepository, IAnswerResultsWriteRepository
    {
        private readonly InMemoryDatastore _datastore;

        public AnswerResultRepository(InMemoryDatastore datastore)
        {
            _datastore = datastore;
        }

        public Task<AnswerResult?> GetAnswerResultByAnswerId(Guid answerId)
        {
            var answerResultsEntity = _datastore.AnswerResults.Values
                .SingleOrDefault(e => e.AnswerId == answerId);
            
            if (answerResultsEntity == default)
            {
                return Task.FromResult(default(AnswerResult));
            }
            
            var answerResults = new AnswerResult(
                answerResultsEntity.Id, 
                answerResultsEntity.QuestionResultId, 
                answerResultsEntity.AnswerId, 
                answerResultsEntity.Votes);
            
            return Task.FromResult((AnswerResult?)answerResults);
        }

        public Task Create(params AnswerResultEntity[] entities)
        {
            var anyExist = entities
                .SingleOrDefault(e => _datastore.AnswerResults.ContainsKey(e.Id));
            
            if (anyExist is not null)
            {
                throw new EntityAlreadyExistsException(
                    typeof(AnswerResultEntity), anyExist.Id);
            }

            foreach (var entity in entities)
            {
                _datastore.AnswerResults.Add(entity.Id, entity);
            }
            
            return Task.CompletedTask;
        }

        public Task Update(AnswerResultEntity entity)
        {
            if (!_datastore.AnswerResults.ContainsKey(entity.Id))
            {
                throw new EntityNotFoundException(
                    typeof(AnswerResultEntity), entity.Id);
            }
            
            _datastore.AnswerResults[entity.Id] = entity;
            
            return Task.CompletedTask;
        }
    }
}