using System;
using System.Threading.Tasks;
using VotingSystem.Core.Models;

namespace VotingSystem.Core.InfrastructureAbstractions
{
    public interface IAnswersRepository
    {
        Task<AnswerModel?> Get(Guid answerId);
    }
}