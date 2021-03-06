﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SharpDomain.Application;
using SharpDomain.Responses;
using VotingSystem.Application.Voters.ViewModels;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Voters;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Voters
{
    internal class LogInHandler : IQueryHandler<LogIn, VoterViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IVotersRepository _votersRepository;

        public LogInHandler(IMapper mapper, IVotersRepository votersRepository)
        {
            _mapper = mapper;
            _votersRepository = votersRepository;
        }

        public async Task<Response<VoterViewModel>> Handle(LogIn request, CancellationToken cancellationToken)
        {
            var pesel = Pesel.ValidateAndCreate(request.Pesel);
            
            var voter = await _votersRepository.GetByPesel(pesel);
            
            if (voter is null)
            {
                return new AuthenticationError(
                    "login with the given data has failed");
            }
            
            var viewModel = _mapper.Map<Voter, VoterViewModel>(voter);
            return viewModel;
        }
    }
}