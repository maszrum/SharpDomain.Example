using Autofac;
using Npgsql;

namespace VotingSystem.Persistence.Dapper
{
    public static class AutofacExtensions
    {
        public static ContainerBuilder RegisterDapperPersistence(this ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterGeneric(typeof(DatabaseConnectionBehavior<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            
            containerBuilder
                .Register(context => new NpgsqlConnection()) // TODO: provide connection string
                .AsSelf()
                .InstancePerLifetimeScope();
            
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
    }
}