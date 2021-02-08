namespace VotingSystem.Persistence.Dapper
{
    internal delegate string SchemaProvider();
    
    internal class DatabaseConfiguration
    {
        public string ConnectionString { get; init; } = 
            "Server=localhost;Port=5432;Database=voting_system;User Id=db_user;Password=db_pass;";
        
        public string Schema { get; init; } = 
            "public";
    }
}