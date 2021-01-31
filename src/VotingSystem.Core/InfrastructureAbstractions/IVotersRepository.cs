using System;
using System.Threading.Tasks;
using VotingSystem.Core.Voter;

namespace VotingSystem.Core.InfrastructureAbstractions
{
    public interface IVotersRepository
    {
        Task<Voter.Voter?> Get(Guid voterId);
        Task<Voter.Voter?> GetByPesel(Pesel pesel);
        Task<bool> Exists(string pesel);
        Task<int> GetCount();
    }
}