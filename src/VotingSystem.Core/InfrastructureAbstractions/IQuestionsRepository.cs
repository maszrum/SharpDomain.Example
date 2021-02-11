using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VotingSystem.Core.Questions;

namespace VotingSystem.Core.InfrastructureAbstractions
{
    public interface IQuestionsRepository
    {
        Task<Question?> Get(Guid questionId);
        Task<IReadOnlyList<Question>> GetAll();
        Task<int> GetCount();
    }
}