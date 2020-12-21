using System.Linq;
using AutoMapper;
using VotingSystem.Application.ViewModels;
using VotingSystem.Core.Models;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Mappers
{
    internal class QuestionToQuestionResultViewModel : Profile
    {
        public QuestionToQuestionResultViewModel()
        {
            CreateMap<Question, QuestionResultViewModel>()
                .ForMember(
                    dest => dest.QuestionId, 
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.Answers,
                    opt => opt.MapFrom(src => src.Answers
                        .Select(a => new AnswerResultViewModel(a.Id, a.Votes))));
        }
    }
}