using System;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.Core.Answer;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.InMemory.Datastore;
using VotingSystem.Persistence.InMemory.Exceptions;
using VotingSystem.Persistence.RepositoryInterfaces;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.InMemory.Repositories
{
    internal class AnswersRepository : IAnswersWriteRepository, IAnswersRepository
    {
        private readonly InMemoryDatastore _datastore;

        public AnswersRepository(InMemoryDatastore datastore)
        {
            _datastore = datastore;
        }

        public Task<Answer?> Get(Guid answerId)
        {
            if (_datastore.Answers.TryGetValue(answerId, out var entity))
            {
                var answer = new Answer(
                    entity.Id,
                    entity.QuestionId, 
                    entity.Order, 
                    entity.Text, 
                    entity.Votes);
                
                return Task.FromResult((Answer?)answer);
            }
            
            return Task.FromResult(default(Answer));
        }
        
        public Task Create(params AnswerEntity[] answers)
        {
            var anyExist = answers.SingleOrDefault(a => _datastore.Answers.ContainsKey(a.Id));
            if (anyExist is not null)
            {
                throw new EntityAlreadyExistsException(
                    typeof(AnswerEntity), anyExist.Id);
            }
            
            foreach (var answer in answers)
            {
                _datastore.Answers.Add(answer.Id, answer);
            }
            
            return Task.CompletedTask;
        }

        public Task Update(AnswerEntity answer)
        {
            if (!_datastore.Answers.ContainsKey(answer.Id))
            {
                throw new EntityNotFoundException(
                    typeof(AnswerEntity), answer.Id);
            }
            
            _datastore.Answers[answer.Id] = answer;
            
            return Task.CompletedTask;
        }
    }
}