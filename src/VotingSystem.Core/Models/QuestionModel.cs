using System;
using System.Collections.Generic;
using System.Linq;
using SharpDomain.Core;
using VotingSystem.Core.Events;
using VotingSystem.Core.Exceptions;

namespace VotingSystem.Core.Models
{
    public class QuestionModel : AggregateRoot
    {
        public const int MinimumAnswers = 2;

        public QuestionModel(
            Guid id, 
            string questionText, 
            IEnumerable<AnswerModel> answers)
        {
            Id = id;
            QuestionText = questionText;
            _answers = answers as List<AnswerModel> ?? answers.ToList();
        }
        
        public override Guid Id { get; }
        
        public string QuestionText { get; }
        
        private readonly List<AnswerModel> _answers;
        public IReadOnlyList<AnswerModel> Answers => _answers;
        
        public static QuestionModel Create(string questionText, IEnumerable<string> answers)
        {
            var questionId = Guid.NewGuid();
            
            var answerModels = answers.Select(
                    (answerText, index) =>
                    {
                        var answerId = Guid.NewGuid();
                        var order = index + 1;
                        var questionAnswer = new AnswerModel(answerId, questionId, order, answerText, 0);
                        return questionAnswer;
                    })
                .ToList();
            
            if (answerModels.Count < MinimumAnswers)
            {
                throw new TooFewAnswersInQuestionException(
                    minimumAnswers: MinimumAnswers);
            }
            
            var question = new QuestionModel(questionId, questionText, answerModels);
            
            var createdEvent = new QuestionCreated(questionId);
            question.Events.Add(createdEvent);
            
            return question;
        }
    }
}