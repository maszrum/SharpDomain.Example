﻿using FluentValidation;
using VotingSystem.Core.Questions;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Questions.Validators
{
    internal class CreateQuestionValidator : AbstractValidator<CreateQuestion>
    {
        public CreateQuestionValidator()
        {
            RuleFor(x => x.QuestionText).NotEmpty();
            
            RuleFor(x => x.Answers).NotNull();
            RuleFor(x => x.Answers.Count).GreaterThanOrEqualTo(Question.MinimumAnswers);
            RuleForEach(x => x.Answers).NotEmpty();
        }
    }
}