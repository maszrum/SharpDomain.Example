﻿using System;
using System.Collections.Generic;
using System.Linq;
using VotingSystem.Core.Vote;

namespace VotingSystem.Application.Voters.ViewModels
{
    public class MyVotesViewModel
    {
        public List<Guid> QuestionsId { get; } = new();

        public override string ToString()
        {
            var questionsId = string.Join(
                Environment.NewLine, 
                QuestionsId.Select(id => $"  {nameof(Vote.QuestionId)}: {id}"));
            return $"# MyVotes{Environment.NewLine}{questionsId}";
        }
    }
}