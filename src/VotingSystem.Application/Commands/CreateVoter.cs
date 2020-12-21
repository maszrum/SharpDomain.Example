using SharpDomain.Application;
using VotingSystem.Application.ViewModels;

namespace VotingSystem.Application.Commands
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