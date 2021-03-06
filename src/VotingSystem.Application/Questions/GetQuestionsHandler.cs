﻿using System.Collections.Generic;
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
            
            var viewModel = _mapper.Map<IEnumerable<Question>, QuestionsListViewModel>(questions);
            
            return viewModel;
        }
    }
}