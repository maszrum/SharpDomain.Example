using System.Collections.Generic;
using System.Threading.Tasks;
using VotingSystem.Persistence.Entities;

namespace VotingSystem.Persistence.RepositoryInterfaces
{
    public interface IAnswersWriteRepository
    {
        Task Create(params AnswerEntity[] answers);
        Task Update(AnswerEntity answer, IReadOnlyList<string> changedProperties);
    }
}