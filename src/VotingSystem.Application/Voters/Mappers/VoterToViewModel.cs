using AutoMapper;
using VotingSystem.Application.Voters.ViewModels;
using VotingSystem.Core.Voters;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Voters.Mappers
{
    internal class VoterToViewModel : Profile
    {
        public VoterToViewModel()
        {
            CreateMap<Voter, VoterViewModel>();
        }
    }
}