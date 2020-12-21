using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.Application.Commands;
using VotingSystem.Application.Queries;
using VotingSystem.WebApi.SharpDomain;

namespace VotingSystem.WebApi.Controllers
{
    // first version
    // TODO: do it as it should be (with jwt)
    
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : DomainController
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] LogIn request)
        {
            var response = await _mediator.Send(request);
            
            return HandleErrors(response, Ok);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateVoter request)
        {
            var response = await _mediator.Send(request);
            
            return HandleErrors(response, Ok);
        }
    }
}