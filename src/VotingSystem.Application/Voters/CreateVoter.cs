using SharpDomain.Application;

// ReSharper disable once ClassNeverInstantiated.Global

namespace VotingSystem.Application.Voters
{
    public class CreateVoter : ICreateCommand
    {
        public CreateVoter(string pesel)
        {
            Pesel = pesel;
        }

        public string Pesel { get; }
    }
}