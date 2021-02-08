namespace VotingSystem.Persistence.Dapper
{
    // ReSharper disable once ClassNeverInstantiated.Global
    
    public class DatabaseConfiguration
    {
        public string ConnectionString { get; init; } = 
            "Server=localhost;Port=5432;Database=voting_system;User Id=db_user;Password=db_pass;";
        
        public string Schema { get; init; } = 
            "public";
    }
    
    internal delegate string SchemaProvider();
}