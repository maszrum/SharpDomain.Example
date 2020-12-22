using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SharpDomain.Application;
using VotingSystem.Application.Identity;
using VotingSystem.Application.ViewModels;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Models;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Queries
{
    internal class GetMyVotesHandler : IQueryHandler<GetMyVotes, MyVotesViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IVotesRepository _votesRepository;
        private readonly IIdentityService<VoterIdentity> _identityService;

        public GetMyVotesHandler(
            IMapper mapper, 
            IVotesRepository votesRepository, 
            IIdentityService<VoterIdentity> identityService)
        {
            _votesRepository = votesRepository;
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<Response<MyVotesViewModel>> Handle(GetMyVotes request, CancellationToken cancellationToken)
        {
            // TODO: add authorization (must be logged in)
            
            var identity = _identityService.GetIdentity();
            var votes = await _votesRepository.GetByVoter(identity.Id);
            
            var viewModel = _mapper.Map<IEnumerable<Vote>, MyVotesViewModel>(votes);
            
            return viewModel;
        }
    }
}