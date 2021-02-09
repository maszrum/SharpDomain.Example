// #define IN_MEMORY_PERSISTENCE
#define DAPPER_PERSISTENCE

using SharpDomain.AccessControl;
using SharpDomain.AutoMapper;
using SharpDomain.AutoTransaction;
using SharpDomain.FluentValidation;
using SharpDomain.IoC;
using SharpDomain.IoC.Application;
using SharpDomain.IoC.Core;
using SharpDomain.IoC.Persistence;
using VotingSystem.Application.Questions;
using VotingSystem.Core.Question;
using VotingSystem.Persistence.Entities;

#if IN_MEMORY_PERSISTENCE
using VotingSystem.Persistence.InMemory;
#elif DAPPER_PERSISTENCE
using VotingSystem.Persistence.Dapper;
#endif

namespace VotingSystem.IoC
{
    public class VotingSystemBuilder : SystemBuilder
    {
        public override SystemBuilder WireUpApplication()
        {
            var domainAssembly = typeof(Question).Assembly;
            var applicationAssembly = typeof(CreateQuestion).Assembly;
            var persistenceAssembly = typeof(QuestionEntity).Assembly;
#if IN_MEMORY_PERSISTENCE
            var inMemoryPersistenceAssembly = typeof(Persistence.InMemory.AutofacExtensions).Assembly;
#elif DAPPER_PERSISTENCE
            var dapperPersistenceAssembly = typeof(DatabaseConfiguration).Assembly;
#endif
            
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
#if IN_MEMORY_PERSISTENCE
                .RegisterInMemoryPersistence()
                .RegisterAutoTransaction(inMemoryPersistenceAssembly);
#elif DAPPER_PERSISTENCE
                .RegisterDapperPersistence(_ => new DatabaseConfiguration())
                .RegisterAutoTransaction(dapperPersistenceAssembly);
#endif

            return this;
        }
    }
}