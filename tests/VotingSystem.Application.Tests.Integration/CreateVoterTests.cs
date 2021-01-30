using System.Threading.Tasks;
using NUnit.Framework;
using SharpDomain.Responses;
using VotingSystem.Application.Tests.Integration.TestBase;
using VotingSystem.Application.Voter;

namespace VotingSystem.Application.Tests.Integration
{
    [TestFixture]
    public class CreateVoterTests : IntegrationTestBase
    {
        [Test]
        public async Task Check_If_Voter_Was_Created()
        {
            const string voterPesel = "87062537594";
            
            var createVoter = new CreateVoter(voterPesel);
            _ = await Mediator.Send(createVoter);
            
            var logIn = new LogIn(voterPesel);
            var voterResponse = await Mediator.Send(logIn);
            
            var voter = AssertNotError.Of(voterResponse);
            Assert.That(voter.Pesel, Is.EqualTo(voterPesel));
        }
        
        [Test]
        public async Task Should_Return_Error_On_Duplicated_Voter()
        {
            const string voterPesel = "87062537594";
            
            var createFirstVoter = new CreateVoter(voterPesel);
            var firstResponse = await Mediator.Send(createFirstVoter);
            
            var createSameAsPreviousVoter = new CreateVoter(voterPesel);
            var secondResponse = await Mediator.Send(createSameAsPreviousVoter);

            AssertNotError.Of(firstResponse);
            AssertError<ObjectAlreadyExistsError>.Of(secondResponse);
        }
        
        [Test]
        public async Task First_Registered_Voter_Should_Be_Admin()
        {
            const string voterPesel = "87062537594";
            
            var createVoter = new CreateVoter(voterPesel);
            var voterResponse = await Mediator.Send(createVoter);
            
            var voter = AssertNotError.Of(voterResponse);
            Assert.That(voter.IsAdministrator, Is.True);
        }
        
        [Test]
        public async Task Second_Registered_Voter_Should_Not_Be_Admin()
        {
            const string firstVoterPesel = "87062537594";
            const string secondVoterPesel = "92113059581";
            
            var createFirstVoter = new CreateVoter(firstVoterPesel);
            _ = await Mediator.Send(createFirstVoter);
            
            var createSecondVoter = new CreateVoter(secondVoterPesel);
            var secondVoterResponse = await Mediator.Send(createSecondVoter);
            
            var secondVoter = AssertNotError.Of(secondVoterResponse);
            Assert.That(secondVoter.IsAdministrator, Is.False);
        }
    }
}