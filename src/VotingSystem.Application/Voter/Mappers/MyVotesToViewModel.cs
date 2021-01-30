using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using VotingSystem.Application.Voter.ViewModels;
using VotingSystem.Core.Models;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Voter.Mappers
{
    internal class MyVotesToViewModel : Profile
    {
        public MyVotesToViewModel()
        {
            CreateMap<IEnumerable<VoteModel>, MyVotesViewModel>()
                .ForMember(
                    dest => dest.QuestionsId,
                    opt => opt.MapFrom(src => src.Select(v => v.QuestionId)));
        }
    }
}