using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.Application.Commands;
using VotingSystem.Application.Queries;

namespace VotingSystem.WebApi.Controllers
{
    // first version
    // TODO: do it as it should be (with jwt)
    
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
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
            
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateVoter request)
        {
            var response = await _mediator.Send(request);
            
            return Ok(response);
        }
    }
}