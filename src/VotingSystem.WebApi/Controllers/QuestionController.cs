using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.Application.Commands;
using VotingSystem.Application.Queries;
using VotingSystem.WebApi.SharpDomain;

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
            var response = await _mediator.Send(request);
            
            return HandleErrors(response, Ok);
        }
        
        // TODO: remove voterId param and get it from jwt
        [HttpGet("result/{questionId}/{voterId}")]
        public async Task<IActionResult> Get(
            [FromRoute] Guid questionId, 
            [FromRoute] Guid voterId)
        {
            var request = new GetQuestionResult(questionId, voterId);
            var response = await _mediator.Send(request);
            
            return HandleErrors(response, Ok);
        }
    }
}