using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SharpDomain.Infrastructure;
using VotingSystem.Core.Vote;
using VotingSystem.Core.Voter;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.RepositoryInterfaces;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.EventHandlers
{
    internal class VotePostedHandler : InfrastructureHandler<VotePosted, VoterModel>
    {
        private readonly IMapper _mapper;
        private readonly IVotesWriteRepository _votesWriteRepository;

        public VotePostedHandler(IMapper mapper, IVotesWriteRepository votesWriteRepository)
        {
            _mapper = mapper;
            _votesWriteRepository = votesWriteRepository;
        }

        public override Task Handle(VotePosted @event, VoterModel model, CancellationToken cancellationToken)
        {
            var vote = model.Votes
                .Single(v => v.Id == @event.VoteId);
            
            var voteEntity = _mapper.Map<VoteModel, VoteEntity>(vote);
            
            return _votesWriteRepository.Create(voteEntity);
        }
    }
}