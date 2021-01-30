using System.Threading.Tasks;
using NUnit.Framework;
using SharpDomain.Responses;
using VotingSystem.Application.Question;
using VotingSystem.Application.Tests.Integration.TestBase;
using VotingSystem.Application.Voter;

namespace VotingSystem.Application.Tests.Integration
{
    [TestFixture]
    public class VoteForTests : IntegrationTestBase
    {
        [Test]
        public async Task Should_Return_Error_When_Not_Logged()
        {
            await LogInAsAdministrator();
            
            var createQuestion = new CreateQuestion(
                questionText: "Some question?", 
                answers: new[] {"First answer", "Second answer"});
            var createQuestionResponse = await Mediator.Send(createQuestion);
            var createdQuestion = AssertNotError.Of(createQuestionResponse);
            
            LogOut();
            
            var voteFor = new VoteFor(
                questionId: createdQuestion.Id, 
                answerId: createdQuestion.Answers[1].Id);
            var voteForResponse = await Mediator.Send(voteFor);
            
            AssertError<AuthorizationError>.Of(voteForResponse);
        }
    
        [Test]
        public async Task Check_If_Votes_Incremented()
        {
            await LogInAsAdministrator();
            
            var createQuestion = new CreateQuestion(
                questionText: "Some question?", 
                answers: new[] {"First answer", "Second answer"});
            var createQuestionResponse = await Mediator.Send(createQuestion);
            var createdQuestion = AssertNotError.Of(createQuestionResponse);
            
            await LogInAsVoter();
            
            var voteFor = new VoteFor(
                questionId: createdQuestion.Id, 
                answerId: createdQuestion.Answers[1].Id);
            var voteForResponse = await Mediator.Send(voteFor);
            
            var getQuestionResult = new GetQuestionResult(createdQuestion.Id);
            var questionResultResponse = await Mediator.Send(getQuestionResult);
            
            AssertNotError.Of(voteForResponse);
            var questionResult = AssertNotError.Of(questionResultResponse);
            
            Assert.Multiple(() =>
            {
                Assert.That(questionResult.Answers[0].Votes, Is.EqualTo(0));
                Assert.That(questionResult.Answers[1].Votes, Is.EqualTo(1));
            });
        }
        
        [Test]
        public async Task Should_Return_Error_On_Twice_Vote_On_Same_Question()
        {
            await LogInAsAdministrator();
            
            var createQuestion = new CreateQuestion(
                questionText: "Some question?", 
                answers: new[] {"First answer", "Second answer"});
            var createQuestionResponse = await Mediator.Send(createQuestion);
            var createdQuestion = AssertNotError.Of(createQuestionResponse);
            
            await LogInAsVoter();
            
            var voteFor = new VoteFor(
                questionId: createdQuestion.Id, 
                answerId: createdQuestion.Answers[1].Id);
            var voteForResponse = await Mediator.Send(voteFor);
            
            var voteForDuplicate = new VoteFor(
                questionId: createdQuestion.Id, 
                answerId: createdQuestion.Answers[0].Id);
            var voteForResponseDuplicate = await Mediator.Send(voteForDuplicate);
            
            AssertError<DomainError>.Of(voteForResponseDuplicate);
        }
    }
}