using System.Collections.Generic;
using AutoMapper;
using VotingSystem.Application.Question.ViewModels;
using VotingSystem.Core.Models;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Question.Mappers
{
    internal class QuestionsListToViewModel : Profile
    {
        public QuestionsListToViewModel()
        {
            CreateMap<IEnumerable<QuestionModel>, QuestionsListViewModel>()
                .ForMember(
                    dest => dest.Questions, 
                    opt => opt.MapFrom(src => src));
        }
    }
}