using FluentValidation;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Voters.Validators
{
    internal class VoteForValidator : AbstractValidator<VoteFor>
    {
        public VoteForValidator()
        {
            RuleFor(x => x.AnswerId).NotEmpty();
            RuleFor(x => x.QuestionId).NotEmpty();
        }
    }
}