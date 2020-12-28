﻿using Autofac;
using SharpDomain.AccessControl;
using SharpDomain.AutoMapper;
using SharpDomain.AutoTransaction;
using SharpDomain.FluentValidation;
using SharpDomain.IoC.Application;
using SharpDomain.IoC.Core;
using SharpDomain.IoC.Persistence;
using VotingSystem.Application.Commands;
using VotingSystem.Core.Models;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.InMemory;

namespace VotingSystem.WebApi.VotingSystem
{
    internal static class VotingSystem
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
                .RegisterAuthorization(applicationAssembly)
                .RegisterPersistenceLayer(persistenceAssembly)
                .RegisterInMemoryPersistence()
                .RegisterAutoTransaction(inMemoryPersistenceAssembly);
            
            return containerBuilder;
        }
    }
}