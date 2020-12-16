using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SharpDomain.Persistence;
using VotingSystem.Core.Events;
using VotingSystem.Core.Models;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.RepositoryInterfaces;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Persistence.EventHandlers
{
    internal class VotePostedHandler : InfrastructureHandler<VotePosted, Vote>
    {
        private readonly IMapper _mapper;
        private readonly IVotesWriteRepository _votesWriteRepository;

        public VotePostedHandler(IMapper mapper, IVotesWriteRepository votesWriteRepository)
        {
            _mapper = mapper;
            _votesWriteRepository = votesWriteRepository;
        }

        public override Task Handle(VotePosted @event, Vote model, CancellationToken cancellationToken)
        {
            var voteEntity = _mapper.Map<Vote, VoteEntity>(model);
            
            return _votesWriteRepository.Create(voteEntity);
        }
    }
}