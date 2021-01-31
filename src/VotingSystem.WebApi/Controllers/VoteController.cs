using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.Application.Voters;

namespace VotingSystem.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VoteController : DomainController
    {
        private readonly IMediator _mediator;

        public VoteController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        public async Task<IActionResult> Vote([FromBody] VoteFor request)
        {
            var response = await _mediator.Send(request);
            
            return HandleErrors(response, _ => Ok());
        }
    }
}