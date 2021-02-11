using AutoMapper;
using VotingSystem.Core.Votes;
using VotingSystem.Persistence.Entities;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.Mappers
{
    internal class VoteToEntity : Profile
    {
        public VoteToEntity()
        {
            CreateMap<Vote, VoteEntity>();
        }
    }
}