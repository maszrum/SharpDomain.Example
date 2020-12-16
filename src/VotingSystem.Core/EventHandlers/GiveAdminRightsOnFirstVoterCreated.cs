using System.Threading;
using System.Threading.Tasks;
using SharpDomain.Core;
using VotingSystem.Core.Events;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Models;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Core.EventHandlers
{
    internal class GiveAdminRightsOnFirstVoterCreated : DomainEventHandler<VoterCreated, Voter>
    {
        private readonly IDomainEvents _domainEvents;
        private readonly IVotersRepository _votersRepository;

        public GiveAdminRightsOnFirstVoterCreated(IDomainEvents domainEvents, IVotersRepository votersRepository)
        {
            _domainEvents = domainEvents;
            _votersRepository = votersRepository;
        }

        public override async Task Handle(VoterCreated @event, Voter model, CancellationToken cancellationToken)
        {
            var votersCount = await _votersRepository.GetCount();
            if (votersCount == 1)
            {
                using (_domainEvents.CollectPropertiesChange(model))
                {
                    model.IsAdministrator = true;
                }
                
                await _domainEvents.PublishCollected(cancellationToken);
            }
        }
    }
}