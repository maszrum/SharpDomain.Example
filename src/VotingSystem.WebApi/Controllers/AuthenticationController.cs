using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharpDomain.Application;
using VotingSystem.Application.Commands;
using VotingSystem.Application.Identity;
using VotingSystem.Application.Queries;
using VotingSystem.WebApi.SharpDomain;

namespace VotingSystem.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : DomainController
    {
        private readonly IMediator _mediator;
        private readonly IAuthenticationService<VoterIdentity> _authenticationService;

        public AuthenticationController(
            IMediator mediator, 
            IAuthenticationService<VoterIdentity> authenticationService)
        {
            _mediator = mediator;
            _authenticationService = authenticationService;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] LogIn request)
        {
            var response = await _mediator.Send(request);
            
            if (!response.TryGet(out var voterViewModel))
            {
                return HandleErrors(response, Ok);
            }
            
            var voterIdentity = new VoterIdentity(
                voterViewModel.Id, 
                voterViewModel.Pesel, 
                voterViewModel.IsAdministrator);
            
            var token = _authenticationService.GenerateToken(voterIdentity);
            
            var logInResponse = new
            {
                Token = token,
                Voter = voterViewModel
            };
            
            return Ok(logInResponse);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateVoter request)
        {
            var response = await _mediator.Send(request);
            
            return HandleErrors(response, Ok);
        }
    }
}