using FluentValidation;
using VotingSystem.Core.Question;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Question.Validators
{
    internal class CreateQuestionValidator : AbstractValidator<CreateQuestion>
    {
        public CreateQuestionValidator()
        {
            RuleFor(x => x.QuestionText).NotEmpty();
            
            RuleFor(x => x.Answers).NotNull();
            RuleFor(x => x.Answers.Count).GreaterThanOrEqualTo(QuestionModel.MinimumAnswers);
            RuleForEach(x => x.Answers).NotEmpty();
        }
    }
}