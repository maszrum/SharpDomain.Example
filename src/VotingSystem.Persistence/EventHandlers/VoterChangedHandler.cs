using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SharpDomain.Core;
using SharpDomain.Infrastructure;
using VotingSystem.Core.Voter;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.RepositoryInterfaces;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.EventHandlers
{
    internal class VoterChangedHandler : InfrastructureHandler<ModelChanged<VoterModel>, VoterModel>
    {
        private static readonly string[] ValidPropertiesChange = { nameof(VoterModel.IsAdministrator) };
        
        private readonly IMapper _mapper;
        private readonly IVotersWriteRepository _votersWriteRepository;

        public VoterChangedHandler(
            IMapper mapper, 
            IVotersWriteRepository votersWriteRepository)
        {
            _mapper = mapper;
            _votersWriteRepository = votersWriteRepository;
        }

        public override Task Handle(ModelChanged<VoterModel> @event, VoterModel model, CancellationToken cancellationToken)
        {
            var invalidPropertyChanged = @event.PropertiesChanged
                .FirstOrDefault(p => !ValidPropertiesChange.Contains(p));
            
            if (!string.IsNullOrEmpty(invalidPropertyChanged))
            {
                throw new InvalidOperationException(
                    $"invalid property changed in {nameof(VoterModel)}: {invalidPropertyChanged}");
            }
            
            var voterEntity = _mapper.Map<VoterModel, VoterEntity>(model);
            
            return _votersWriteRepository.Update(voterEntity);
        }
    }
}