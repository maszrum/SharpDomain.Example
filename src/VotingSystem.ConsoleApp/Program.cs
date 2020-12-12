﻿using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using SharpDomain.Application;
using SharpDomain.AutoMapper;
using SharpDomain.AutoTransaction;
using SharpDomain.Core;
using SharpDomain.FluentValidation;
using SharpDomain.Persistence;
using VotingSystem.Application.Commands;
using VotingSystem.ConsoleApp.CommandLine;
using VotingSystem.Core.Models;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.InMemory;

// ReSharper disable once ClassNeverInstantiated.Global

namespace VotingSystem.ConsoleApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var domainAssembly = typeof(Question).Assembly;
            var applicationAssembly = typeof(CreateQuestion).Assembly;
            var persistenceAssembly = typeof(QuestionEntity).Assembly;
            var inMemoryPersistenceAssembly = typeof(Persistence.InMemory.AutofacExtensions).Assembly;

            var containerBuilder = new ContainerBuilder()
                .RegisterDomainLayer(domainAssembly)
                .RegisterApplicationLayer(
                    assembly: applicationAssembly,
                    configurationAction: config =>
                    {
                        config.ForbidMediatorInHandlers = true;
                        config.ForbidWriteRepositoriesInHandlersExceptIn(persistenceAssembly);
                    })
                .RegisterFluentValidation(applicationAssembly)
                .RegisterAutoMapper(applicationAssembly, persistenceAssembly)
                .RegisterPersistenceLayer(persistenceAssembly)
                .RegisterInMemoryPersistence()
                .RegisterAutoTransaction(inMemoryPersistenceAssembly);

            containerBuilder.RegisterClientDependencies();

            await using var container = containerBuilder.Build();

            await SeedDataForTesting(container);

            var tcs = new CancellationTokenSource();
            var consoleTask = RunConsole(container)
                .ContinueWith(task =>
                {
                    tcs.Cancel();
                    
                    if (task.IsFaulted && task.Exception != default)
                    {
                        ExceptionDispatchInfo.Capture(task.Exception).Throw();
                    }
                }, CancellationToken.None);
            
            var simulationTask = SimulateVoting(container, tcs.Token);
            
            try
            {
                await Task.WhenAll(consoleTask, simulationTask);
            }
            catch (OperationCanceledException) {}
        }

        private static Task RunConsole(IContainer container)
        {
            var consoleVoter = new ConsoleVoter(container);
            return Task.Run(() => consoleVoter.RunBlocking());
        }
        
        private static async Task SimulateVoting(IContainer container, CancellationToken cancellationToken)
        {
            var simulatedVoter = new SimulatedVoter(container);
            
            while (!cancellationToken.IsCancellationRequested)
            {
                await simulatedVoter.LogAsRandomVoter();
                await simulatedVoter.VoteRandomly();
                simulatedVoter.Logout();
                
                await Task.Delay(200, cancellationToken);
            }
        }

        private static async Task SeedDataForTesting(IContainer container)
        {
            await using var scope = container.BeginLifetimeScope();
            var mediator = scope.Resolve<IMediator>();

            var createVoter = new CreateVoter("12312312312");
            await mediator.Send(createVoter);

            var createQuestion = new CreateQuestion("Some question?", new[] { "Answer 1", "Answer 2", "Answer 3" });
            await mediator.Send(createQuestion);
        }
    }
}