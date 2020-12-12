using FluentValidation;
using VotingSystem.Application.Commands;
using VotingSystem.Core.Models;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Validators
{
    internal class CreateQuestionValidator : AbstractValidator<CreateQuestion>
    {
        public CreateQuestionValidator()
        {
            RuleFor(x => x.QuestionText).NotEmpty();
            
            RuleFor(x => x.Answers).NotNull();
            RuleFor(x => x.Answers.Count).GreaterThanOrEqualTo(Question.MinimmumAnswers);
            RuleForEach(x => x.Answers).NotEmpty();
        }
    }
}