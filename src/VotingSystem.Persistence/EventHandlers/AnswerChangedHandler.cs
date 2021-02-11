using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SharpDomain.Core;
using SharpDomain.Infrastructure;
using VotingSystem.Core.Answers;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.RepositoryInterfaces;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.EventHandlers
{
    internal class AnswerChangedHandler : InfrastructureHandler<ModelChanged<Answer>, Answer>
    {
        private static readonly string[] ValidPropertiesChange = { nameof(Answer.Votes) };
        
        private readonly IMapper _mapper;
        private readonly IAnswersWriteRepository _answersWriteRepository;

        public AnswerChangedHandler(
            IMapper mapper, 
            IAnswersWriteRepository answersWriteRepository)
        {
            _mapper = mapper;
            _answersWriteRepository = answersWriteRepository;
        }

        public override Task Handle(ModelChanged<Answer> @event, Answer model, CancellationToken cancellationToken)
        {
            var invalidPropertyChanged = @event.PropertiesChanged
                .FirstOrDefault(p => !ValidPropertiesChange.Contains(p));
            
            if (!string.IsNullOrEmpty(invalidPropertyChanged))
            {
                throw new InvalidOperationException(
                    $"invalid property changed in {nameof(Answer)}: {invalidPropertyChanged}");
            }
            
            var answerEntity = _mapper.Map<Answer, AnswerEntity>(model);
            
            return _answersWriteRepository.Update(answerEntity, @event.PropertiesChanged);
        }
    }
}