using System.Threading.Tasks;
using SharpDomain.IoC;

namespace VotingSystem.Persistence.Dapper
{
    public class DatabaseInitializer : ISystemInitializer
    {
        public Task InitializeIfNeed()
        {
            // TODO: create tables if does not exist
            return Task.CompletedTask;
        }

        public Task InitializeForcefully()
        {
            // TODO: remove tables and create
            return Task.CompletedTask;
        }
    }
}