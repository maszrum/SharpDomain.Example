using SharpDomain.AccessControl;
using SharpDomain.AutoMapper;
using SharpDomain.AutoTransaction;
using SharpDomain.FluentValidation;
using SharpDomain.IoC;
using SharpDomain.IoC.Application;
using SharpDomain.IoC.Core;
using SharpDomain.IoC.Persistence;
using VotingSystem.Application.Question;
using VotingSystem.Core.Question;
using VotingSystem.Persistence.Entities;
using VotingSystem.Persistence.InMemory;

namespace VotingSystem.IoC
{
    public class VotingSystemBuilder : SystemBuilder
    {
        public override SystemBuilder WireUpApplication()
        {
            var domainAssembly = typeof(QuestionModel).Assembly;
            var applicationAssembly = typeof(CreateQuestion).Assembly;
            var persistenceAssembly = typeof(QuestionEntity).Assembly;
            var inMemoryPersistenceAssembly = typeof(Persistence.InMemory.AutofacExtensions).Assembly;
            
            ContainerBuilder
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
            
            return this;
        }
    }
}