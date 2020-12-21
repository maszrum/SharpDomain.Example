using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharpDomain.Application;
using SharpDomain.Errors;

namespace VotingSystem.WebApi.SharpDomain
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
                    _ when IsDomainError(error, out var exceptionType) => UnprocessableEntity(new { exceptionType, error.Message }),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                },
                onSuccess);
        }
        
        private static bool IsDomainError(ErrorBase error, out string exceptionType)
        {
            var errorType = error.GetType();
            
            if (errorType.GetGenericTypeDefinition() == typeof(DomainError<>))
            {
                exceptionType = errorType.Name;
                return true;
            }
            
            exceptionType = default;
            return false;
        }
    }
}