﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SharpDomain.AccessControl;
using SharpDomain.Application;
using SharpDomain.Core;
using SharpDomain.Responses;
using VotingSystem.Application.Authorization;
using VotingSystem.Core.Questions;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Questions
{
    internal class CreateQuestionHandler : ICreateCommandHandler<CreateQuestion>, IAuthorizationRequired
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

        public async Task<Response<Guid>> Handle(CreateQuestion request, CancellationToken cancellationToken)
        {
            var question = Question.Create(request.QuestionText, request.Answers);
            
            await _domainEvents
                .CollectFrom(question)
                .PublishCollected(cancellationToken);
            
            return question.Id;
        }
    }
}