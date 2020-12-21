using System;
using System.Threading.Tasks;
using VotingSystem.Core.Models;

namespace VotingSystem.Core.InfrastructureAbstractions
{
    public interface IAnswersRepository
    {
        Task<Answer?> Get(Guid answerId);
    }
}