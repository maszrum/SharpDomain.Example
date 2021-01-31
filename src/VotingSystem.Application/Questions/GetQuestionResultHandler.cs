using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SharpDomain.AccessControl;
using SharpDomain.Application;
using SharpDomain.Responses;
using VotingSystem.Application.Authorization;
using VotingSystem.Application.Identity;
using VotingSystem.Application.Questions.ViewModels;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Question;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Questions
{
    internal class GetQuestionResultHandler : IQueryHandler<GetQuestionResult, QuestionResultViewModel>, IAuthorizable
    {
        private readonly IMapper _mapper;
        private readonly IQuestionsRepository _questionsRepository;
        private readonly IVotesRepository _votesRepository;
        private readonly IIdentityService<VoterIdentity> _identityService;

        public GetQuestionResultHandler(
            IMapper mapper, 
            IQuestionsRepository questionsRepository, 
            IVotesRepository votesRepository, 
            IIdentityService<VoterIdentity> identityService)
        {
            _mapper = mapper;
            _questionsRepository = questionsRepository;
            _votesRepository = votesRepository;
            _identityService = identityService;
        }

        public void ConfigureAuthorization(AuthorizationConfiguration configuration) =>
            configuration.UseRequirement<VoterMustBeLoggedInRequirement>();

        public async Task<Response<QuestionResultViewModel>> Handle(GetQuestionResult request, CancellationToken cancellationToken)
        {
            var identity = _identityService.GetIdentity();
            var userVotes = await _votesRepository.GetByVoter(identity.Id);
            
            var userVoted = userVotes.Any(v => v.QuestionId == request.QuestionId);
            if (!userVoted)
            {
                return new AuthorizationError(
                    "results are only visible after voting");
            }
            
            var question = await _questionsRepository.Get(request.QuestionId);
            if(question is null)
            {
                return ObjectNotFoundError.CreateFor<Question>(request.QuestionId);
            }
            
            var viewModel = _mapper.Map<Question, QuestionResultViewModel>(question);
            return viewModel;
        }
    }
}