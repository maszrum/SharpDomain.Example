using System;
using System.Threading.Tasks;
using VotingSystem.Core.Voters;

namespace VotingSystem.Core.InfrastructureAbstractions
{
    public interface IVotersRepository
    {
        Task<Voter?> Get(Guid voterId);
        Task<Voter?> GetByPesel(Pesel pesel);
        Task<bool> Exists(string pesel);
        Task<int> GetCount();
    }
}