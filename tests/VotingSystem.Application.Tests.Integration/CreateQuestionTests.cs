using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SharpDomain.NUnit;
using SharpDomain.Responses;
using VotingSystem.Application.Questions;
using VotingSystem.Application.Tests.Integration.TestBase;

namespace VotingSystem.Application.Tests.Integration
{
    [TestFixture]
    public class CreateQuestionTests : VotingSystemIntegrationTest
    {
        [Test]
        public async Task Should_Return_Error_On_Not_Logged_In()
        {
            var createQuestion = new CreateQuestion(
                questionText: "Some question?", 
                answers: new[] {"First answer", "Second answer"});
            var createQuestionResponse = await Mediator.Send(createQuestion);
            
            AssertError<AuthorizationError>.Of(createQuestionResponse);
        }
        
        [Test]
        public async Task Should_Return_Error_On_Non_Admin()
        {
            await LogInAsVoter();
            
            var createQuestion = new CreateQuestion(
                questionText: "Some question?", 
                answers: new[] {"First answer", "Second answer"});
            var createQuestionResponse = await Mediator.Send(createQuestion);
            
            AssertError<AuthorizationError>.Of(createQuestionResponse);
        }
        
        [Test]
        public async Task Check_If_Query_Was_Added()
        {
            await LogInAsAdministrator();
            
            var createQuestion = new CreateQuestion(
                questionText: "Some question?", 
                answers: new[] {"First answer", "Second answer"});
            var createQuestionResponse = await Mediator.Send(createQuestion);
            
            var getQuestions = new GetQuestions();
            var questionsResponse = await Mediator.Send(getQuestions);
            
            var addedQuestionId = AssertNotError.Of(createQuestionResponse);
            var allQuestions = AssertNotError.Of(questionsResponse);
            
            var questionThatWasAdded = allQuestions.Questions.SingleOrDefault(q => q.Id == addedQuestionId);
            
            Assert.That(questionThatWasAdded, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(
                    questionThatWasAdded!.QuestionText, 
                    Is.EqualTo(createQuestion.QuestionText));
                
                CollectionAssert.AreEqual(
                    questionThatWasAdded.Answers.Select(a => a.Text), 
                    createQuestion.Answers);
            });
        }
    }
}