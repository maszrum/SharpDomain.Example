﻿using System;
using SharpDomain.Core;

namespace VotingSystem.Core.Questions
{
    public class QuestionCreated : EventBase
    {
        public QuestionCreated(Guid questionId)
        {
            QuestionId = questionId;
        }
        
        public Guid QuestionId { get; }
    }
}