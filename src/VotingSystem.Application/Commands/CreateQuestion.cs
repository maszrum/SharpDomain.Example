﻿using System.Collections.Generic;
using SharpDomain.Application;
using VotingSystem.Application.ViewModels;

namespace VotingSystem.Application.Commands
{
    public class CreateQuestion : ICommand<QuestionViewModel>
    {
        public CreateQuestion(
            string questionText, 
            IList<string> answers)
        {
            QuestionText = questionText;
            Answers = answers;
        }

        public string QuestionText { get; }
        public IList<string> Answers { get; }
    }
}