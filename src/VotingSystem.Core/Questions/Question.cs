using System;
using System.Collections.Generic;
using System.Linq;
using SharpDomain.Core;
using VotingSystem.Core.Answers;

namespace VotingSystem.Core.Questions
{
    public class Question : AggregateRoot
    {
        public const int MinimumAnswers = 2;

        public Question(
            Guid id, 
            string questionText, 
            IEnumerable<Answer> answers)
        {
            Id = id;
            QuestionText = questionText;
            _answers = answers as List<Answer> ?? answers.ToList();
            
            if (_answers.Count < MinimumAnswers)
            {
                throw new TooFewAnswersInQuestionException(
                    minimumAnswers: MinimumAnswers);
            }
        }
        
        public override Guid Id { get; }
        
        public string QuestionText { get; }
        
        private readonly List<Answer> _answers;
        public IReadOnlyList<Answer> Answers => _answers;
        
        public static Question Create(string questionText, IEnumerable<string> answers)
        {
            var questionId = Guid.NewGuid();
            
            var answerModels = answers.Select(
                    (answerText, index) =>
                    {
                        var answerId = Guid.NewGuid();
                        var order = index + 1;
                        var questionAnswer = new Answer(answerId, questionId, order, answerText, 0);
                        return questionAnswer;
                    })
                .ToList();
            
            if (answerModels.Count < MinimumAnswers)
            {
                throw new TooFewAnswersInQuestionException(
                    minimumAnswers: MinimumAnswers);
            }
            
            var question = new Question(questionId, questionText, answerModels);
            
            var createdEvent = new QuestionCreated(questionId);
            question.Events.Add(createdEvent);
            
            return question;
        }
    }
}