using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using SharpDomain.AccessControl;
using SharpDomain.IoC;
using VotingSystem.Application.Identity;
using VotingSystem.ConsoleApp.CommandLine;
using VotingSystem.IoC;

// ReSharper disable once ClassNeverInstantiated.Global

namespace VotingSystem.ConsoleApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var container = await new VotingSystemBuilder()
                .WireUpApplication()
                .WithIdentityService<AuthenticationService, VoterIdentity>()
                .With(containerBuilder => containerBuilder.RegisterClientDependencies())
                .Initialize(InitializationType.IfNeed)
                .Build();
            
            await container.Seed();
            
            await using (container)
            {
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
        }

        private static Task RunConsole(IContainer container)
        {
            var consoleVoter = new ConsoleVoter(container);
            return Task.Run(consoleVoter.RunBlocking);
        }
        
        private static async Task SimulateVoting(IContainer container, CancellationToken cancellationToken)
        {
            var simulatedVoter = new SimulatedVoter(container);
            
            while (!cancellationToken.IsCancellationRequested)
            {
                await simulatedVoter.LogInAsRandomVoter();
                await simulatedVoter.VoteRandomly();
                simulatedVoter.Logout();
                
                await Task.Delay(200, cancellationToken);
            }
        }
    }
}