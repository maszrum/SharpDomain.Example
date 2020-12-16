using Autofac;
using SharpDomain.Application;
using SharpDomain.AutoMapper;
using SharpDomain.AutoTransaction;
using SharpDomain.Core;
using SharpDomain.FluentValidation;
using SharpDomain.Persistence;
using VotingSystem.Application.Commands;
using VotingSystem.Core.Models;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.InMemory;

namespace VotingSystem.WebApi
{
    public static class VotingSystemBuilder
    {
        public static ContainerBuilder BuildVotingSystem(this ContainerBuilder containerBuilder)
        {
            var domainAssembly = typeof(Question).Assembly;
            var applicationAssembly = typeof(CreateQuestion).Assembly;
            var persistenceAssembly = typeof(QuestionEntity).Assembly;
            var inMemoryPersistenceAssembly = typeof(Persistence.InMemory.AutofacExtensions).Assembly;

            containerBuilder
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
            
            return containerBuilder;
        }
    }
}