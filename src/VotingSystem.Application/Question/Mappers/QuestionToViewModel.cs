using AutoMapper;
using VotingSystem.Application.Question.ViewModels;
using VotingSystem.Core.Answer;
using VotingSystem.Core.Question;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Question.Mappers
{
    internal class QuestionToViewModel : Profile
    {
        public QuestionToViewModel()
        {
            CreateMap<AnswerModel, QuestionViewModel.AnswerViewModel>();
            CreateMap<QuestionModel, QuestionViewModel>();
        }
    }
}