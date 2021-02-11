using AutoMapper;
using VotingSystem.Application.Questions.ViewModels;
using VotingSystem.Core.Answers;
using VotingSystem.Core.Questions;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Questions.Mappers
{
    internal class QuestionToViewModel : Profile
    {
        public QuestionToViewModel()
        {
            CreateMap<Answer, QuestionViewModel.AnswerViewModel>();
            CreateMap<Question, QuestionViewModel>();
        }
    }
}