﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SharpDomain.NUnit;
using VotingSystem.Application.Questions;
using VotingSystem.Application.Tests.Integration.TestBase;

namespace VotingSystem.Application.Tests.Integration
{
    [TestFixture]
    public class GetQuestionsTests : VotingSystemIntegrationTest
    {
        [Test]
        public async Task Create_3_Questions_And_Check_If_Added()
        {
            await LogInAsAdministrator();
            
            var createFirstQuestion = new CreateQuestion(
                questionText: "Some question?", 
                answers: new[] {"First answer", "Second answer"});
            var createFirstQuestionResponse = await Mediator.Send(createFirstQuestion);
            var firstQuestionId = AssertNotError.Of(createFirstQuestionResponse);
            
            var createSecondQuestion = new CreateQuestion(
                questionText: "Another question?", 
                answers: new[] {"Any answer", "Some answer"});
            var createSecondQuestionResponse = await Mediator.Send(createSecondQuestion);
            var secondQuestionId = AssertNotError.Of(createSecondQuestionResponse);
            
            var createThirdQuestion = new CreateQuestion(
                questionText: "Third question?", 
                answers: new[] {"Any answer", "Some answer", "I don't know answer"});
            var createThirdQuestionResponse = await Mediator.Send(createThirdQuestion);
            var thirdQuestionId = AssertNotError.Of(createThirdQuestionResponse);
            
            await LogInAsVoter();
            
            var expectedQuestions = new Dictionary<Guid, CreateQuestion>
            {
                [firstQuestionId] = createFirstQuestion, 
                [secondQuestionId] = createSecondQuestion, 
                [thirdQuestionId] = createThirdQuestion
            };
            
            var getQuestions = new GetQuestions();
            var getQuestionsResponse = await Mediator.Send(getQuestions);
            var questions = AssertNotError.Of(getQuestionsResponse);
            
            Assert.That(questions.Questions, Has.Count.EqualTo(3));
            
            CollectionAssert.AreEquivalent(
                expectedQuestions.Select(q => q.Value.QuestionText),
                questions.Questions.Select(q => q.QuestionText));

            foreach (var question in questions.Questions)
            {
                var expectedAnswers = expectedQuestions
                    .Single(q => q.Key == question.Id)
                    .Value
                    .Answers;
                
                CollectionAssert.AreEqual(
                    expectedAnswers, 
                    question.Answers.Select(a => a.Text));
            }
        }
    }
}