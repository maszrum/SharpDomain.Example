using System.Collections.Generic;
using SharpDomain.Application;

// ReSharper disable once ClassNeverInstantiated.Global

namespace VotingSystem.Application.Questions
{
    public class CreateQuestion : ICreateCommand
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