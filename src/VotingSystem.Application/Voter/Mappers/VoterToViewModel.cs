using AutoMapper;
using VotingSystem.Application.Voter.ViewModels;
using VotingSystem.Core.Models;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Voter.Mappers
{
    internal class VoterToViewModel : Profile
    {
        public VoterToViewModel()
        {
            CreateMap<VoterModel, VoterViewModel>();
        }
    }
}