using System;
using System.Reflection;
using Autofac;
using Npgsql;

namespace VotingSystem.Persistence.Dapper
{
    public static class AutofacExtensions
    {
        public static ContainerBuilder RegisterDapperPersistence(
            this ContainerBuilder containerBuilder, 
            Func<IComponentContext, DatabaseConfiguration> configurationProvider)
        {
            return containerBuilder
                .RegisterConfiguration(configurationProvider)
                .RegisterConnection()
                .RegisterRepositories();
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
                .Register(context =>
                {
                    var configuration = context.Resolve<DatabaseConfiguration>();
                    return configuration.Schema;
                })
                .As<SchemaProvider>()
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
                .Register(context => new NpgsqlConnection()) // TODO: provide connection string
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
    }
}