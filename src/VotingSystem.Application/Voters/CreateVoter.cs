using SharpDomain.Application;
using VotingSystem.Application.Voters.ViewModels;

// ReSharper disable once ClassNeverInstantiated.Global

namespace VotingSystem.Application.Voters
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