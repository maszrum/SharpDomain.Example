using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SharpDomain.Core;
using SharpDomain.Persistence;
using VotingSystem.Core.Models;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.RepositoryInterfaces;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.EventHandlers
{
    internal class AnswerResultChangedHandler : InfrastructureHandler<ModelChanged<AnswerResult>, AnswerResult>
    {
        private static readonly string[] ValidPropertiesChange = { nameof(AnswerResult.Votes) };
        
        private readonly IMapper _mapper;
        private readonly IAnswerResultsWriteRepository _answerResultsWriteRepository;

        public AnswerResultChangedHandler(
            IMapper mapper, 
            IAnswerResultsWriteRepository answerResultsWriteRepository)
        {
            _mapper = mapper;
            _answerResultsWriteRepository = answerResultsWriteRepository;
        }

        public override Task Handle(ModelChanged<AnswerResult> @event, AnswerResult model, CancellationToken cancellationToken)
        {
            var invalidPropertyChanged = @event.PropertiesChanged
                .FirstOrDefault(p => !ValidPropertiesChange.Contains(p));
            
            if (!string.IsNullOrEmpty(invalidPropertyChanged))
            {
                throw new InvalidOperationException(
                    $"invalid property changed in {nameof(AnswerResult)}: {invalidPropertyChanged}");
            }
            
            var answerResultEntity = _mapper.Map<AnswerResult, AnswerResultEntity>(model);
            
            return _answerResultsWriteRepository.Update(answerResultEntity);
        }
    }
}