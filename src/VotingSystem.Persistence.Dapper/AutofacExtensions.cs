using System;
using System.Reflection;
using Autofac;
using Dapper;
using Npgsql;
using VotingSystem.Persistence.Dapper.AutoTransaction;

namespace VotingSystem.Persistence.Dapper
{
    public static class AutofacExtensions
    {
        public static ContainerBuilder RegisterDapperPersistence(
            this ContainerBuilder containerBuilder, 
            Func<IComponentContext, DatabaseConfiguration> configurationProvider)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            
            return containerBuilder
                .RegisterConfiguration(configurationProvider)
                .RegisterConnection()
                .RegisterRepositories()
                .RegisterTransactionProvider()
                .RegisterInitializer();
        }
        
        private static ContainerBuilder RegisterConfiguration(
            this ContainerBuilder containerBuilder, 
            Func<IComponentContext, DatabaseConfiguration> configurationProvider)
        {
            containerBuilder
                .Register(configurationProvider)
                .AsSelf()
                .SingleInstance();
            
            containerBuilder
                .Register<SchemaProvider>(context =>
                {
                    var configuration = context.Resolve<DatabaseConfiguration>();
                    return () => configuration.Schema;
                })
                .AsSelf()
                .SingleInstance();
            
            return containerBuilder;
        }
        
        private static ContainerBuilder RegisterConnection(this ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterGeneric(typeof(DatabaseConnectionBehavior<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            
            containerBuilder
                .Register(context =>
                {
                    var configuration = context.Resolve<DatabaseConfiguration>();
                    return new NpgsqlConnection(configuration.ConnectionString);
                })
                .AsSelf()
                .InstancePerLifetimeScope();
            
            return containerBuilder;
        }
        
        private static ContainerBuilder RegisterRepositories(this ContainerBuilder containerBuilder)
        {
            static bool IsRepositoryType(Type t) => 
                t.Name.ToLowerInvariant().Contains("repository") && !t.IsAbstract;
            
            var assembly = typeof(AutofacExtensions).GetTypeInfo().Assembly;
            
            containerBuilder
                .RegisterAssemblyTypes(assembly)
                .Where(IsRepositoryType)
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();
            
            return containerBuilder;
        }
        
        private static ContainerBuilder RegisterTransactionProvider(this ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterType<TransactionWrapper>()
                .AsSelf()
                .As<ITransactionProvider>()
                .InstancePerLifetimeScope();
            
            return containerBuilder;
        }
        
        private static ContainerBuilder RegisterInitializer(this ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterType<DatabaseInitializer>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            
            return containerBuilder;
        }
    }
}