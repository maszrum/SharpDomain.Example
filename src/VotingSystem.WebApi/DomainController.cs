using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharpDomain.Responses;

namespace VotingSystem.WebApi
{
    public abstract class DomainController : ControllerBase
    {
        protected IActionResult HandleErrors<TData>(Response<TData> response, Func<TData, IActionResult> onSuccess) 
            where TData : class
        {
            return response.Match(
                error => error switch
                {
                    AuthenticationError ae => Unauthorized(ae),
                    AuthorizationError _ => Forbid(),
                    ObjectAlreadyExistsError oaee => Conflict(oaee),
                    ObjectNotFoundError onfe => NotFound(onfe),
                    UserError ue => UnprocessableEntity(ue),
                    ValidationError ve => BadRequest(ve),
                    DomainError de => UnprocessableEntity(new { de.ExceptionType.Name, error.Message }),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                },
                onSuccess);
        }
    }
}