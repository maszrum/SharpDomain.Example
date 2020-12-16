using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.Application.Queries;

namespace VotingSystem.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VoterController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VoterController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        // TODO: remove voterId param and get it from jwt
        [HttpGet("votes/{voterId}")]
        public async Task<IActionResult> GetMyVotes([FromRoute] Guid voterId)
        {
            var request = new GetMyVotes(voterId);
            var response = await _mediator.Send(request);
            
            return Ok(response);
        }
    }
}