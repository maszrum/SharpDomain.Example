using FluentValidation;
using VotingSystem.Application.Commands;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Validators
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