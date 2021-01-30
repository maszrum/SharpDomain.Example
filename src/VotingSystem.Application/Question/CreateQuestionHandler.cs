using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SharpDomain.AccessControl;
using SharpDomain.Application;
using SharpDomain.Core;
using SharpDomain.Responses;
using VotingSystem.Application.Authorization;
using VotingSystem.Application.Question.ViewModels;
using VotingSystem.Core.Question;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Question
{
    internal class CreateQuestionHandler : ICommandHandler<CreateQuestion, QuestionViewModel>, IAuthorizable
    {
        private readonly IMapper _mapper;
        private readonly IDomainEvents _domainEvents;

        public CreateQuestionHandler(
            IMapper mapper, 
            IDomainEvents domainEvents)
        {
            _mapper = mapper;
            _domainEvents = domainEvents;
        }

        public void ConfigureAuthorization(AuthorizationConfiguration configuration) =>
            configuration.UseRequirement<VoterMustBeAdministratorRequirement>();

        public async Task<Response<QuestionViewModel>> Handle(CreateQuestion request, CancellationToken cancellationToken)
        {
            var question = QuestionModel.Create(request.QuestionText, request.Answers);
            
            await _domainEvents
                .CollectFrom(question)
                .PublishCollected(cancellationToken);
            
            var viewModel = _mapper.Map<QuestionModel, QuestionViewModel>(question);
            return viewModel;
        }
    }
}