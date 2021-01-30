using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SharpDomain.Application;
using SharpDomain.Responses;
using VotingSystem.Application.Question.ViewModels;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Question;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Question
{
    internal class GetQuestionsHandler : IQueryHandler<GetQuestions, QuestionsListViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IQuestionsRepository _questionsRepository;

        public GetQuestionsHandler(
            IMapper mapper, 
            IQuestionsRepository questionsRepository)
        {
            _mapper = mapper;
            _questionsRepository = questionsRepository;
        }

        public async Task<Response<QuestionsListViewModel>> Handle(GetQuestions request, CancellationToken cancellationToken)
        {
            var questions = await _questionsRepository.GetAll();
            
            var viewModel = _mapper.Map<IEnumerable<QuestionModel>, QuestionsListViewModel>(questions);
            
            return viewModel;
        }
    }
}