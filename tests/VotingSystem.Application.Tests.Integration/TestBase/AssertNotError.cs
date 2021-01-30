using NUnit.Framework;
using SharpDomain.Responses;

namespace VotingSystem.Application.Tests.Integration.TestBase
{
    internal static class AssertNotError
    {
        public static TData Of<TData>(Response<TData> response) where TData : class
        {
            Assert.That(response.IsError, Is.False);
            
            return response.Data!;
        }
    }
}