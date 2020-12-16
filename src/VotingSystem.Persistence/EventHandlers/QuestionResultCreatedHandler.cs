using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SharpDomain.Persistence;
using VotingSystem.Core.Events;
using VotingSystem.Core.Models;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.RepositoryInterfaces;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.EventHandlers
{
    internal class QuestionResultCreatedHandler : InfrastructureHandler<QuestionResultCreated, QuestionResult>
    {
        private readonly IMapper _mapper;
        private readonly IQuestionResultsWriteRepository _questionResultsWriteRepository;
        private readonly IAnswerResultsWriteRepository _answerResultsWriteRepository;

        public QuestionResultCreatedHandler(
            IMapper mapper, 
            IQuestionResultsWriteRepository questionResultsWriteRepository, 
            IAnswerResultsWriteRepository answerResultsWriteRepository)
        {
            _mapper = mapper;
            _questionResultsWriteRepository = questionResultsWriteRepository;
            _answerResultsWriteRepository = answerResultsWriteRepository;
        }

        public override async Task Handle(QuestionResultCreated @event, QuestionResult model, CancellationToken cancellationToken)
        {
            var questionResultEntity = _mapper.Map<QuestionResult, QuestionResultEntity>(model);
            
            await  _questionResultsWriteRepository.Create(questionResultEntity);

            var answerResultEntities = _mapper
                .Map<IEnumerable<AnswerResult>, IEnumerable<AnswerResultEntity>>(model.AnswerResults)
                .ToArray();

            await _answerResultsWriteRepository.Create(answerResultEntities);
        }
    }
}