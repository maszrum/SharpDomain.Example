using FluentValidation;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Questions.Validators
{
    internal class GetQuestionResultValidator : AbstractValidator<GetQuestionResult>
    {
        public GetQuestionResultValidator()
        {
            RuleFor(x => x.QuestionId).NotEmpty();
        }
    }
}