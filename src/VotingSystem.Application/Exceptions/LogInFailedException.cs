using System;

namespace VotingSystem.Application.Exceptions
{
    internal class LogInFailedException : ApplicationException
    {
        public LogInFailedException() 
            : base("logging in with the given data has failed")
        {
        }
    }
}