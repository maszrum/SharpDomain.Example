using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SharpDomain.Application;
using SharpDomain.Core;
using SharpDomain.Responses;
using VotingSystem.Application.Voter.ViewModels;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Models;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Voter
{
    internal class CreateVoterHandler : ICommandHandler<CreateVoter, VoterViewModel>
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

        public async Task<Response<VoterViewModel>> Handle(CreateVoter request, CancellationToken cancellationToken)
        {
            var exists = await _voters.Exists(request.Pesel);
            if (exists)
            {
                return ObjectAlreadyExistsError.CreateFor<VoterModel>();
            }

            var voter = VoterModel.Create(request.Pesel);

            await _domainEvents
                .CollectFrom(voter)
                .PublishCollected(cancellationToken);

            var viewModel = _mapper.Map<VoterModel, VoterViewModel>(voter);
            return viewModel;
        }
    }
}