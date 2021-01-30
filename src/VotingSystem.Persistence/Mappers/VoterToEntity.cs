using AutoMapper;
using VotingSystem.Core.Voter;
using VotingSystem.Persistence.Entities;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.Mappers
{
    internal class VoterToEntity : Profile
    {
        public VoterToEntity()
        {
            CreateMap<VoterModel, VoterEntity>();
        }
    }
}