using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using SharpDomain.Application;
using VotingSystem.Application.Commands;
using VotingSystem.Application.Queries;

namespace VotingSystem.ConsoleApp
{
    internal class SimulatedVoter
    {
        private readonly IContainer _container;
        private Guid? _voterId;

        public SimulatedVoter(IContainer container)
        {
            _container = container;
        }
        
        public async Task LogAsRandomVoter()
        {
            await using var scope = _container.BeginLifetimeScope();
            var mediator = _container.Resolve<IMediator>();
            
            var voter = await EnsureNotError(() =>
            {
                var pesel = GenerateRandomPesel();
                var createVoter = new CreateVoter(pesel);
                return mediator.Send(createVoter);
            });
            
            var logIn = new LogIn(voter.Pesel);
            var logInResponse = await mediator.Send(logIn)
                .OnError(error => throw new InvalidOperationException(error.ToString()));
            
            _voterId = logInResponse.Id;
        }
        
        public void Logout() => _voterId = default;

        public async Task VoteRandomly()
        {
            if (!_voterId.HasValue)
            {
                throw new InvalidOperationException(
                    $"call {nameof(LogAsRandomVoter)} before");
            }
            
            await using var scope = _container.BeginLifetimeScope();
            var mediator = _container.Resolve<IMediator>();
            
            var getQuestions = new GetQuestions();
            var getQuestionsResponse = await mediator.Send(getQuestions);
            
            var questionsList = getQuestionsResponse
                .OnError(error => throw new InvalidOperationException(error.ToString()));

            foreach (var question in questionsList.Questions)
            {
                var answerGuids = question.Answers
                    .Select(a => a.Id)
                    .ToArray();
                
                var selectedAnswerId = GetRandomGuid(answerGuids);
                
                var voteFor = new VoteFor(
                    voterId: _voterId.Value, 
                    questionId: question.Id,
                    answerId: selectedAnswerId);
                await mediator.Send(voteFor);
            }
        }
        
        private static readonly Random Random = new(DateTime.Now.Millisecond);
        private static string GenerateRandomPesel()
        {
            var year = Random.Next(50, 100).ToString();
            var month = Random.Next(1, 13).ToString("D2");
            var day = Random.Next(1, 32).ToString("D2");
            var rest = Random.Next(1, 99999).ToString("D5");
            return string.Concat(year, month, day, rest);
        }
        
        private static Guid GetRandomGuid(IReadOnlyList<Guid> guids)
        {
            var index = Random.Next(0, guids.Count);
            return guids[index];
        }
        
        private static async Task<TData> EnsureNotError<TData>(Func<Task<Response<TData>>> action) 
            where TData : class
        {
            var tries = 0;
            Response<TData> response;
            TData? data;
            do
            {
                if (tries == 3)
                {
                    throw new InvalidOperationException(
                        $"received error {tries} times when invoking action");
                }
                
                response = await action();
                tries++;
            }
            while (!response.TryGet(out data));
            
            return data;
        }
    }
}