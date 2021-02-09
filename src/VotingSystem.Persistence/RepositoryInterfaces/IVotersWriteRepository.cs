using System.Collections.Generic;
using System.Threading.Tasks;
using VotingSystem.Persistence.Entities;

namespace VotingSystem.Persistence.RepositoryInterfaces
{
    public interface IVotersWriteRepository
    {
        Task Create(VoterEntity voter);
        Task Update(VoterEntity voter, IReadOnlyList<string> changedProperties);
    }
}