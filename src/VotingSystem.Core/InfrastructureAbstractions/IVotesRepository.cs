﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VotingSystem.Core.Vote;

namespace VotingSystem.Core.InfrastructureAbstractions
{
    public interface IVotesRepository
    {
        Task<IReadOnlyList<Vote.Vote>> GetByVoter(Guid voterId);
    }
}