using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SharpDomain.Infrastructure;
using VotingSystem.Core.Events;
using VotingSystem.Core.Models;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.RepositoryInterfaces;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.EventHandlers
{
    internal class VoterCreatedHandler : InfrastructureHandler<VoterCreated, VoterModel>
    {
        private readonly IMapper _mapper;
        private readonly IVotersWriteRepository _votersWriteRepository;

        public VoterCreatedHandler(
            IMapper mapper, 
            IVotersWriteRepository votersWriteRepository)
        {
            _mapper = mapper;
            _votersWriteRepository = votersWriteRepository;
        }

        public override Task Handle(VoterCreated @event, VoterModel model, CancellationToken cancellationToken)
        {
            var voterEntity = _mapper.Map<VoterModel, VoterEntity>(model);
            
            return _votersWriteRepository.Create(voterEntity);
        }
    }
}