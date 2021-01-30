using SharpDomain.Application;
using VotingSystem.Application.Voter.ViewModels;

// ReSharper disable once ClassNeverInstantiated.Global

namespace VotingSystem.Application.Voter
{
    public class CreateVoter : ICommand<VoterViewModel>
    {
        public CreateVoter(string pesel)
        {
            Pesel = pesel;
        }

        public string Pesel { get; }
    }
}