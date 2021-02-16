using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SharpDomain.Application;
using SharpDomain.Responses;
using VotingSystem.Application.Questions.ViewModels;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Questions;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Questions
{
    internal class GetQuestionHandler : IQueryHandler<GetQuestion, QuestionViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IQuestionsRepository _questionsRepository;

        public GetQuestionHandler(
            IMapper mapper, 
            IQuestionsRepository questionsRepository)
        {
            _mapper = mapper;
            _questionsRepository = questionsRepository;
        }

        public async Task<Response<QuestionViewModel>> Handle(GetQuestion request, CancellationToken cancellationToken)
        {
            var question = await _questionsRepository.Get(request.Id);
            
            if (question is null)
            {
                return ObjectNotFoundError
                    .CreateFor<Question>(request.Id);
            }
            
            var viewModel = _mapper.Map<Question, QuestionViewModel>(question);
            
            return viewModel;
        }
    }
}