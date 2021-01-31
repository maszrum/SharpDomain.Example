using System.Linq;
using FluentValidation;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Voters.Validators
{
    internal class LogInValidator : AbstractValidator<LogIn>
    {
        public LogInValidator()
        {
            RuleFor(x => x.Pesel)
                .NotNull()
                .Length(11)
                .Must(pesel => pesel.All(char.IsDigit));
        }
    }
}