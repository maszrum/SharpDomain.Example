using FluentValidation;
using VotingSystem.Application.Queries;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Validators
{
    internal class GetQuestionResultValidator : AbstractValidator<GetQuestionResult>
    {
        public GetQuestionResultValidator()
        {
            RuleFor(x => x.QuestionId).NotEmpty();
        }
    }
}