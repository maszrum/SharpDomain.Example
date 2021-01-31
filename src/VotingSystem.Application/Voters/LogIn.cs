using SharpDomain.Application;
using VotingSystem.Application.Voters.ViewModels;

// ReSharper disable once ClassNeverInstantiated.Global

namespace VotingSystem.Application.Voters
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