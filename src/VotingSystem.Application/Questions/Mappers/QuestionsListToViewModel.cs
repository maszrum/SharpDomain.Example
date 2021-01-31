using System.Collections.Generic;
using AutoMapper;
using VotingSystem.Application.Questions.ViewModels;
using VotingSystem.Core.Question;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Questions.Mappers
{
    internal class QuestionsListToViewModel : Profile
    {
        public QuestionsListToViewModel()
        {
            CreateMap<IEnumerable<Question>, QuestionsListViewModel>()
                .ForMember(
                    dest => dest.Questions, 
                    opt => opt.MapFrom(src => src));
        }
    }
}