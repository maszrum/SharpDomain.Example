using SharpDomain.Application;
using VotingSystem.Application.Voter.ViewModels;

// ReSharper disable once ClassNeverInstantiated.Global

namespace VotingSystem.Application.Voter
{
    public class LogIn : IQuery<VoterViewModel>
    {
        public LogIn(string pesel)
        {
            Pesel = pesel;
        }

        public string Pesel { get; }
    }
}