using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.Application.Queries;
using VotingSystem.WebApi.SharpDomain;

namespace VotingSystem.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VoterController : DomainController
    {
        private readonly IMediator _mediator;

        public VoterController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet("votes")]
        public async Task<IActionResult> GetMyVotes()
        {
            var request = new GetMyVotes();
            var response = await _mediator.Send(request);
            
            return HandleErrors(response, Ok);
        }
    }
}