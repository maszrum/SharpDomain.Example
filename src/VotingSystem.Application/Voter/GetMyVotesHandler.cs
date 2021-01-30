using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SharpDomain.AccessControl;
using SharpDomain.Application;
using SharpDomain.Responses;
using VotingSystem.Application.Authorization;
using VotingSystem.Application.Identity;
using VotingSystem.Application.Voter.ViewModels;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Models;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Voter
{
    internal class GetMyVotesHandler : IQueryHandler<GetMyVotes, MyVotesViewModel>, IAuthorizable
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

        public void ConfigureAuthorization(AuthorizationConfiguration configuration) =>
            configuration.UseRequirement<VoterMustBeLoggedInRequirement>();

        public async Task<Response<MyVotesViewModel>> Handle(GetMyVotes request, CancellationToken cancellationToken)
        {
            var identity = _identityService.GetIdentity();
            var votes = await _votesRepository.GetByVoter(identity.Id);
            
            var viewModel = _mapper.Map<IEnumerable<VoteModel>, MyVotesViewModel>(votes);
            
            return viewModel;
        }
    }
}