﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using SharpDomain.Application;
using VotingSystem.Application.Identity;
using VotingSystem.Application.Questions;
using VotingSystem.Application.Voters;

namespace VotingSystem.ConsoleApp
{
    internal class SimulatedVoter
    {
        private readonly IContainer _container;
        private VoterIdentity? _voterIdentity;

        public SimulatedVoter(IContainer container)
        {
            _container = container;
        }
        
        public async Task LogInAsRandomVoter()
        {
            await using var scope = _container.BeginLifetimeScope();
            var mediator = _container.Resolve<IMediator>();
            
            var pesel = GenerateRandomPesel();
            
            var createVoter = new CreateVoter(pesel);
            await mediator.Send(createVoter)
                .OnError(error => throw new InvalidOperationException(error.ToString()));
            
            var logIn = new LogIn(pesel);
            var logInResponse = await mediator.Send(logIn)
                .OnError(error => throw new InvalidOperationException(error.ToString()));

            _voterIdentity = new VoterIdentity(
                logInResponse.Id, 
                logInResponse.Pesel, 
                logInResponse.IsAdministrator);
        }

        public void Logout() => _voterIdentity = default;

        public async Task VoteRandomly()
        {
            if (_voterIdentity is null)
            {
                throw new InvalidOperationException(
                    $"call {nameof(LogInAsRandomVoter)} before");
            }

            await using var scope = _container.BeginLifetimeScope();
            var mediator = scope.Resolve<IMediator>();

            var authenticationService = scope.Resolve<AuthenticationService>();
            authenticationService.SetIdentity(_voterIdentity);

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
                    questionId: question.Id,
                    answerId: selectedAnswerId);

                await mediator.Send(voteFor)
                    .OnError(error => throw new InvalidOperationException(error.ToString()));
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
    }
}