﻿using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.Application.Questions;

namespace VotingSystem.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : DomainController
    {
        private readonly IMediator _mediator;

        public QuestionController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var request = new GetQuestions();
            var response = await _mediator.Send(request);
            
            return HandleErrors(response, Ok);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateQuestion request)
        {
            var questionId = await _mediator.Send(request);
            
            var getQuestion = new GetQuestion(questionId);
            var response = await _mediator.Send(getQuestion);
            
            return HandleErrors(response, Ok);
        }
        
        [HttpGet("result/{questionId}")]
        public async Task<IActionResult> Get([FromRoute] Guid questionId)
        {
            var request = new GetQuestionResult(questionId);
            var response = await _mediator.Send(request);
            
            return HandleErrors(response, Ok);
        }
    }
}