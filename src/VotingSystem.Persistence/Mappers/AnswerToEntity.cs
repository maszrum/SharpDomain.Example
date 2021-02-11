using AutoMapper;
using VotingSystem.Core.Answers;
using VotingSystem.Persistence.Entities;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.Mappers
{
    internal class AnswerToEntity : Profile
    {
        public AnswerToEntity()
        {
            CreateMap<Answer, AnswerEntity>();
        }
    }
}