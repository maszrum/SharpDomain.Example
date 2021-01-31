using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VotingSystem.Core.Question;

namespace VotingSystem.Core.InfrastructureAbstractions
{
    public interface IQuestionsRepository
    {
        Task<Question.Question?> Get(Guid questionId);
        Task<IReadOnlyList<Question.Question>> GetAll();
    }
}