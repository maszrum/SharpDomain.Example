using System;
using System.Threading;
using System.Threading.Tasks;
using SharpDomain.Core;
using VotingSystem.Core.InfrastructureAbstractions;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Core.Voters
{
    internal class GiveAdminRightsOnFirstVoterCreated : IEventHandler<VoterCreated>
    {
        private readonly IDomainEvents _domainEvents;
        private readonly IVotersRepository _votersRepository;

        public GiveAdminRightsOnFirstVoterCreated(IDomainEvents domainEvents, IVotersRepository votersRepository)
        {
            _domainEvents = domainEvents;
            _votersRepository = votersRepository;
        }

        public async Task Handle(VoterCreated notification, CancellationToken cancellationToken)
        {
            var votersCount = await _votersRepository.GetCount();
            if (votersCount == 1)
            {
                var voter = await _votersRepository.Get(notification.VoterId);
                if (voter is null)
                {
                    throw new NullReferenceException(
                        $"cannot get voter in {nameof(GiveAdminRightsOnFirstVoterCreated)}");
                }
                
                using (_domainEvents.CollectPropertiesChange(voter))
                {
                    voter.IsAdministrator = true;
                }
                
                await _domainEvents.PublishCollected(cancellationToken);
            }
        }
    }
}