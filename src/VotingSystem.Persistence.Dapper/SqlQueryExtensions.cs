using System;
using System.Linq;

namespace VotingSystem.Persistence.Dapper
{
    internal static class SqlQueryExtensions
    {
        public static string InjectSchema(this string sql, string schemaName) => 
            sql.Replace("@Schema", schemaName);
        
        public static string PascalCaseToSnakeCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(input), "cannot be empty");
            }
            
            var numberOfCapitals = input.Count(char.IsUpper);
            var resultLength = input.Length + numberOfCapitals - 1;
            var outArray = new char[resultLength];

            outArray[0] = char.ToLower(input[0]);
            var outI = 1;
            for (var i = 1; i < input.Length; i++, outI++)
            {
                var character = input[i];
                if (char.IsUpper(character))
                {
                    outArray[outI] = '_';
                    outArray[outI + 1] = char.ToLower(character);
                    outI++;
                }
                else
                {
                    outArray[outI] = character;
                }
            }
            
            return new string(outArray);
        }
    }
}