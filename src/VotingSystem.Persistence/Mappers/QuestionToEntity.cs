using AutoMapper;
using VotingSystem.Core.Questions;
using VotingSystem.Persistence.Entities;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.Mappers
{
    internal class QuestionToEntity : Profile
    {
        public QuestionToEntity()
        {
            CreateMap<Question, QuestionEntity>();
        }
    }
}