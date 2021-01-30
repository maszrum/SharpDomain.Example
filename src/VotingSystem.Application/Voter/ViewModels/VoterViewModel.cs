using System;
using System.Text;
using VotingSystem.Core.Models;

namespace VotingSystem.Application.Voter.ViewModels
{
    public class VoterViewModel
    {
        public VoterViewModel(
            Guid id, 
            string pesel, 
            bool isAdministrator)
        {
            Id = id;
            Pesel = pesel;
            IsAdministrator = isAdministrator;
        }

        public Guid Id { get; }
        public string Pesel { get; }
        public bool IsAdministrator { get; }

        public override string ToString()
        {
            return new StringBuilder()
                .AppendLine($"# {nameof(VoterModel)}")
                .AppendLine($"{nameof(Id)}: {Id}")
                .Append($"{nameof(Pesel)}: {Pesel}")
                .ToString();
        }
    }
}