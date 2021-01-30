using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VotingSystem.Core.Question;

namespace VotingSystem.Core.InfrastructureAbstractions
{
    public interface IQuestionsRepository
    {
        Task<QuestionModel?> Get(Guid questionId);
        Task<IReadOnlyList<QuestionModel>> GetAll();
    }
}