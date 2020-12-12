using System;
namespace VotingSystem.Persistence.InMemory.Exceptions
{
    internal class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException(Type objectType, Guid id)
            : base($"object of type {objectType.FullName} with id {id} already exists")
        {
        }
    }
}
