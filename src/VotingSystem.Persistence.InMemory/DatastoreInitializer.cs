using System.Threading.Tasks;
using SharpDomain.IoC;
using VotingSystem.Persistence.InMemory.Datastore;

namespace VotingSystem.Persistence.InMemory
{
    internal class DatastoreInitializer : SystemInitializer
    {
        private readonly InMemoryDatastore _datastore;

        public DatastoreInitializer(InMemoryDatastore datastore)
        {
            _datastore = datastore;
        }

        protected override Task InitializeIfNeed()
        {
            // here could be code that creates tables in the database in case it does not exist
            // but this is just a persistence simulation so the data is volatile
            // this class is for presentation purposes only
            
            _datastore.Answers.Clear();
            _datastore.Questions.Clear();
            _datastore.Voters.Clear();
            _datastore.Votes.Clear();
            
            return Task.CompletedTask;
        }

        protected override Task InitializeForcefully()
        {
            // here could be the code responsible for clearing the database
            // and building it anew
            // this class is for presentation purposes only
            
            _datastore.Answers.Clear();
            _datastore.Questions.Clear();
            _datastore.Voters.Clear();
            _datastore.Votes.Clear();
            
            return Task.CompletedTask;
        }
    }
}