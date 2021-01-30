using System.Threading.Tasks;
using NUnit.Framework;
using SharpDomain.Responses;
using VotingSystem.Application.Question;
using VotingSystem.Application.Tests.Integration.TestBase;
using VotingSystem.Application.Voter;

namespace VotingSystem.Application.Tests.Integration
{
    [TestFixture]
    public class GetQuestionResultTests : IntegrationTestBase
    {
        [Test]
        public async Task Should_Return_Error_When_Request_For_Result_Of_Not_Voted_Question()
        {
            await LogInAsAdministrator();
            
            var createQuestion = new CreateQuestion(
                questionText: "Some question?", 
                answers: new[] {"First answer", "Second answer"});
            var createQuestionResponse = await Mediator.Send(createQuestion);
            var question = AssertNotError.Of(createQuestionResponse);
            
            await LogInAsVoter();
            
            var getResult = new GetQuestionResult(question.Id);
            var getResultResponse = await Mediator.Send(getResult);
            
            AssertError<AuthorizationError>.Of(getResultResponse);
        }
        
        [Test]
        public async Task Check_If_Question_Results_Are_Valid()
        {
            await LogInAsAdministrator();
            
            var createFirstQuestion = new CreateQuestion(
                questionText: "Some question?", 
                answers: new[] {"First answer", "Second answer"});
            var createFirstQuestionResponse = await Mediator.Send(createFirstQuestion);
            var firstQuestion = AssertNotError.Of(createFirstQuestionResponse);
            
            var createSecondQuestion = new CreateQuestion(
                questionText: "Another question?", 
                answers: new[] {"Any answer", "Some answer"});
            var createSecondQuestionResponse = await Mediator.Send(createSecondQuestion);
            var secondQuestion = AssertNotError.Of(createSecondQuestionResponse);
            
            var firstVoteFor = new VoteFor(firstQuestion.Id, firstQuestion.Answers[0].Id);
            await Mediator.Send(firstVoteFor);
            
            var secondVoteFor = new VoteFor(secondQuestion.Id, secondQuestion.Answers[1].Id);
            await Mediator.Send(secondVoteFor);
            
            await LogInAsVoter();
            
            var thirdVoteFor = new VoteFor(firstQuestion.Id, firstQuestion.Answers[0].Id);
            await Mediator.Send(thirdVoteFor);
            
            var fourthVoteFor = new VoteFor(secondQuestion.Id, secondQuestion.Answers[0].Id);
            await Mediator.Send(fourthVoteFor);
            
            var getFirstResult = new GetQuestionResult(firstQuestion.Id);
            var firstResultResponse = await Mediator.Send(getFirstResult);
            var firstResult = AssertNotError.Of(firstResultResponse);
            
            var getSecondResult = new GetQuestionResult(secondQuestion.Id);
            var secondResultResponse = await Mediator.Send(getSecondResult);
            var secondResult = AssertNotError.Of(secondResultResponse);
            
            Assert.Multiple(() =>
            {
                Assert.That(firstResult.Answers[0].Votes, Is.EqualTo(2));
                Assert.That(firstResult.Answers[1].Votes, Is.EqualTo(0));
            
                Assert.That(secondResult.Answers[0].Votes, Is.EqualTo(1));
                Assert.That(secondResult.Answers[1].Votes, Is.EqualTo(1));
            });
        }
    }
}