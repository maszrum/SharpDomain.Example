using System.Linq;
using AutoMapper;
using VotingSystem.Application.Question.ViewModels;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Question.Mappers
{
    internal class QuestionToQuestionResultViewModel : Profile
    {
        public QuestionToQuestionResultViewModel()
        {
            CreateMap<Core.Models.QuestionModel, QuestionResultViewModel>()
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