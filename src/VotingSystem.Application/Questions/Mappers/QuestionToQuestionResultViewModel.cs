using System.Linq;
using AutoMapper;
using VotingSystem.Application.Questions.ViewModels;
using VotingSystem.Core.Question;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Questions.Mappers
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