using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SharpDomain.Application;
using SharpDomain.Core;
using SharpDomain.Responses;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Voters;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Voters
{
    internal class CreateVoterHandler : ICreateCommandHandler<CreateVoter>
    {
        private readonly IMapper _mapper;
        private readonly IVotersRepository _voters;
        private readonly IDomainEvents _domainEvents;

        public CreateVoterHandler(
            IMapper mapper, 
            IVotersRepository voters, 
            IDomainEvents domainEvents)
        {
            _mapper = mapper;
            _voters = voters;
            _domainEvents = domainEvents;
        }

        public async Task<Response<Guid>> Handle(CreateVoter request, CancellationToken cancellationToken)
        {
            var exists = await _voters.Exists(request.Pesel);
            if (exists)
            {
                return ObjectAlreadyExistsError.CreateFor<Voter>();
            }

            var voter = Voter.Create(request.Pesel);

            await _domainEvents
                .CollectFrom(voter)
                .PublishCollected(cancellationToken);

            return voter.Id;
        }
    }
}