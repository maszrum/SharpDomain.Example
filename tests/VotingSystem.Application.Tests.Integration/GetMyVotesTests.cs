using System.Threading.Tasks;
using NUnit.Framework;
using SharpDomain.NUnit;
using SharpDomain.Responses;
using VotingSystem.Application.Questions;
using VotingSystem.Application.Tests.Integration.TestBase;
using VotingSystem.Application.Voters;

namespace VotingSystem.Application.Tests.Integration
{
    [TestFixture]
    public class GetMyVotesTests : VotingSystemIntegrationTest
    {
        [Test]
        public async Task Should_Return_Error_When_Not_Logged()
        {
            var getMyVotes = new GetMyVotes();
            var getMyVotesResponse = await Mediator.Send(getMyVotes);
            
            AssertError<AuthorizationError>.Of(getMyVotesResponse);
        }
        
        [Test]
        public async Task Check_If_Votes_Exists_In_My_Votes_After_Voting_2_Of_3_Questions()
        {
            await LogInAsAdministrator();
            
            var createFirstQuestion = new CreateQuestion(
                questionText: "Some question?", 
                answers: new[] {"First answer", "Second answer"});
            var createFirstQuestionResponse = await Mediator.Send(createFirstQuestion);
            var firstQuestionId = AssertNotError.Of(createFirstQuestionResponse);
            
            var getFirstQuestion = new GetQuestion(firstQuestionId);
            var firstQuestionResponse = await Mediator.Send(getFirstQuestion);
            var firstQuestion = AssertNotError.Of(firstQuestionResponse);
            
            var createSecondQuestion = new CreateQuestion(
                questionText: "Another question?", 
                answers: new[] {"Any answer", "Some answer"});
            var createSecondQuestionResponse = await Mediator.Send(createSecondQuestion);
            _ = AssertNotError.Of(createSecondQuestionResponse); // I will not vote on this question
            
            var createThirdQuestion = new CreateQuestion(
                questionText: "Third question?", 
                answers: new[] {"Any answer", "Some answer", "I don't know answer"});
            var createThirdQuestionResponse = await Mediator.Send(createThirdQuestion);
            var thirdQuestionId = AssertNotError.Of(createThirdQuestionResponse);
            
            var getThirdQuestion = new GetQuestion(thirdQuestionId);
            var thirdQuestionResponse = await Mediator.Send(getThirdQuestion);
            var thirdQuestion = AssertNotError.Of(thirdQuestionResponse);
            
            await LogInAsVoter();
            
            var voteForFirst = new VoteFor(
                questionId: firstQuestion.Id, 
                answerId: firstQuestion.Answers[0].Id);
            await Mediator.Send(voteForFirst);
            
            var voteForThird = new VoteFor(
                questionId: thirdQuestion.Id,
                answerId: thirdQuestion.Answers[2].Id);
            await Mediator.Send(voteForThird);
            
            var getMyVotes = new GetMyVotes();
            var getMyVotesResponse = await Mediator.Send(getMyVotes);
            
            var myVotes = AssertNotError.Of(getMyVotesResponse);
            CollectionAssert.AreEquivalent(
                new[] { firstQuestion.Id, thirdQuestion.Id },
                myVotes.QuestionsId);
        }
    }
}