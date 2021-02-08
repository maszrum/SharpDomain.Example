namespace VotingSystem.Persistence.Dapper
{
    internal static class SchemaExtension
    {
        public static string InjectSchema(this string sql, string schemaName) => 
            sql.Replace("@Schema", schemaName);
    }
}