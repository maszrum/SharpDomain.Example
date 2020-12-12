using System;
namespace VotingSystem.Persistence.InMemory.Exceptions
{
    internal class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(Type objectType, Guid id)
            : base($"object of type {objectType.FullName} with id {id} was not found")
        {
        }
    }
}
