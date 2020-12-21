using SharpDomain.Application;
using VotingSystem.Application.ViewModels;

namespace VotingSystem.Application.Queries
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