using System.Threading.Tasks;
using SharpDomain.IoC;

namespace VotingSystem.Persistence.InMemory
{
    internal class DatastoreInitializer : ISystemInitializer
    {
        public Task InitializeIfNeed()
        {
            // here could be code that creates tables in the database in case it does not exist
            // but this is just a persistence simulation so the data is volatile
            // this class is for presentation purposes only
            return Task.CompletedTask;
        }

        public Task InitializeForcefully()
        {
            // here could be the code responsible for clearing the database
            // and building it anew
            // this class is for presentation purposes only
            return Task.CompletedTask;
        }
    }
}