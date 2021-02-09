using System.Threading;
using System.Threading.Tasks;
using SharpDomain.Application;
using SharpDomain.Responses;
using VotingSystem.Application.Questions.ViewModels;
using VotingSystem.Core.InfrastructureAbstractions;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Questions
{
    internal class GetQuestionsCountHandler : IQueryHandler<GetQuestionsCount, QuestionsCountViewModel>
    {
        private readonly IQuestionsRepository _questionsRepository;

        public GetQuestionsCountHandler(IQuestionsRepository questionsRepository)
        {
            _questionsRepository = questionsRepository;
        }

        public async Task<Response<QuestionsCountViewModel>> Handle(GetQuestionsCount request, CancellationToken cancellationToken)
        {
            var count = await _questionsRepository.GetCount();
            
            return new QuestionsCountViewModel(count);
        }
    }
}