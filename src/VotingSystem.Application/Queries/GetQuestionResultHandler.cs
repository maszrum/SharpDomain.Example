using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using VotingSystem.Application.Exceptions;
using VotingSystem.Application.ViewModels;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Models;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Queries
{
    internal class GetQuestionResultHandler : IRequestHandler<GetQuestionResult, QuestionResultViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IQuestionResultsRepository _questionResultsRepository;
        private readonly IVotesRepository _votesRepository;

        public GetQuestionResultHandler(
            IMapper mapper, 
            IQuestionResultsRepository questionResultsRepository, 
            IVotesRepository votesRepository)
        {
            _mapper = mapper;
            _questionResultsRepository = questionResultsRepository;
            _votesRepository = votesRepository;
        }

        public async Task<QuestionResultViewModel> Handle(GetQuestionResult request, CancellationToken cancellationToken)
        {
            await ThrowIfUserNotVoted(request);
            
            var questionResult = await _questionResultsRepository.GetQuestionResultByQuestionId(request.QuestionId);
            
            if (questionResult is null)
            {
                throw new ObjectNotFoundException<Question>(request.QuestionId);
            }
            
            var viewModel = _mapper.Map<QuestionResult, QuestionResultViewModel>(questionResult);
            
            return viewModel;
        }

        private async Task ThrowIfUserNotVoted(GetQuestionResult request)
        {
            var userVotes = await _votesRepository.GetByVoter(request.VoterId);
            var userVoted = userVotes.Any(v => v.QuestionId == request.QuestionId);
            if (!userVoted)
            {
                throw new DenialOfAccessToVotingResultsException(
                    request.VoterId, request.QuestionId);
            }
        }
    }
}