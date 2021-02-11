using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingSystem.Core.Answers;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Questions;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.InMemory.Datastore;
using VotingSystem.Persistence.InMemory.Exceptions;
using VotingSystem.Persistence.RepositoryInterfaces;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.InMemory.Repositories
{
    internal class QuestionsRepository : IQuestionsRepository, IQuestionsWriteRepository
    {
        private readonly InMemoryDatastore _datastore;

        public QuestionsRepository(InMemoryDatastore datastore)
        {
            _datastore = datastore;
        }

        public Task<Question?> Get(Guid questionId)
        {
            if (_datastore.Questions.TryGetValue(questionId, out var entity))
            {
                var answers = _datastore.Answers.Values
                    .Where(e => e.QuestionId == questionId)
                    .Select(a => new Answer(a.Id, a.QuestionId, a.AnswerOrder, a.Text, a.Votes))
                    .ToList();
                
                var question = new Question(
                    entity.Id, 
                    entity.QuestionText, 
                    answers);
                
                return Task.FromResult((Question?)question);
            }
            
            return Task.FromResult(default(Question));
        }

        public Task<IReadOnlyList<Question>> GetAll()
        {
            Dictionary<Guid, List<AnswerEntity>> answerEntitiesGrouped = _datastore.Answers.Values
                .GroupBy(e => e.QuestionId)
                .ToDictionary(g => g.Key, g => g.ToList());
            
            IEnumerable<AnswerEntity> GetAnswersInQuestion(Guid questionId) =>
                answerEntitiesGrouped.TryGetValue(questionId, out var result) 
                    ? result 
                    : Array.Empty<AnswerEntity>();

            var questionEntities = _datastore.Questions.Values;
            
            var questions = questionEntities
                .Select(qe =>
                {
                    var answerEntities = GetAnswersInQuestion(qe.Id);
                    var answers = answerEntities.Select(ae => new Answer(
                        ae.Id, 
                        ae.QuestionId, 
                        ae.AnswerOrder, 
                        ae.Text, 
                        ae.Votes));
                    
                    return new Question(qe.Id, qe.QuestionText, answers);
                })
                .ToList();
            
            return Task.FromResult((IReadOnlyList<Question>)questions);
        }

        public Task<int> GetCount() => 
            Task.FromResult(_datastore.Questions.Count);

        public Task Create(QuestionEntity question)
        {
            if (_datastore.Questions.ContainsKey(question.Id))
            {
                throw new EntityAlreadyExistsException(
                    typeof(QuestionEntity), question.Id);
            }
            
            _datastore.Questions.Add(question.Id, question);
            
            return Task.CompletedTask;
        }
    }
}