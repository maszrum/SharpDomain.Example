using System;
using System.Collections.Generic;
using System.Linq;
using SharpDomain.Core;
using VotingSystem.Core.Events;

namespace VotingSystem.Core.Models
{
    public class QuestionResult : AggregateRoot
    {
        public QuestionResult(
            Guid id, 
            Guid questionId, 
            IEnumerable<AnswerResult> answerResults)
        {
            Id = id;
            QuestionId = questionId;
            AnswerResults = answerResults as IReadOnlyList<AnswerResult> ?? answerResults.ToList();
        }
        
        public override Guid Id { get; }

        public Guid QuestionId { get; }
        
        public IReadOnlyList<AnswerResult> AnswerResults { get; }
        
        public static QuestionResult CreateFromQuestion(Question question)
        {
            var questionResultId = Guid.NewGuid();
            
            var answerResults = question.Answers.Select(answer =>
            {
                var answerResultId = Guid.NewGuid();
                var answerResultModel = new AnswerResult(answerResultId, questionResultId, answer.Id, 0);
                return answerResultModel;
            });
            
            var questionResult = new QuestionResult(questionResultId, question.Id, answerResults);
            
            var createdEvent = new QuestionResultCreated(questionResultId, question.Id);
            questionResult.Events.Append(createdEvent);
            
            return questionResult;
        }
    }
}