﻿using System;
using System.Threading.Tasks;
using VotingSystem.Core.Voter;

namespace VotingSystem.Core.InfrastructureAbstractions
{
    public interface IVotersRepository
    {
        Task<VoterModel?> Get(Guid voterId);
        Task<VoterModel?> GetByPesel(Pesel pesel);
        Task<bool> Exists(string pesel);
        Task<int> GetCount();
    }
}