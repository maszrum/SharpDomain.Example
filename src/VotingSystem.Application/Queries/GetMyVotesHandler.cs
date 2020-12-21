﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SharpDomain.Application;
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

        public GetMyVotesHandler(IMapper mapper, IVotesRepository votesRepository)
        {
            _votesRepository = votesRepository;
            _mapper = mapper;
        }

        public async Task<Response<MyVotesViewModel>> Handle(GetMyVotes request, CancellationToken cancellationToken)
        {
            var votes = await _votesRepository.GetByVoter(request.VoterId);
            
            var viewModel = _mapper.Map<IEnumerable<Vote>, MyVotesViewModel>(votes);
            
            return viewModel;
        }
    }
}