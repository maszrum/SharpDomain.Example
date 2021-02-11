# Voting system

Example application that uses SharpDomain library.

## Console application

Available commands:

```
add-question [question text]
get-questions
get-result
login [pesel]
logout
register [pesel]
vote
```

## ASP.NET Core WebApi application

Documented with Swagger.

## Persistence

There are two types of persistence.

### Volatile in-memory dummy of persistence (default)

Code is in project [VotingSystem.Persistence.InMemory](src/VotingSystem.Persistence.InMemory).

### PostgreSQL database coded with Dapper mapper

To enable PostgreSQL in application go to [this file](src/VotingSystem.IoC/VotingSystemBuilder.cs) and:
1. Comment line ``#define IN_MEMORY_PERSISTENCE``.
2. Uncomment line ``#define DAPPER_PERSISTENCE``.
3. Run database with Docker - [instruction](src/VotingSystem.Persistence.Dapper/.postgresql-docker). It contains the docker-compose file of PostgreSQL database and instructions on how to run it. If you don't have Docker - install PostgreSQL on your computer if you like to clutter it up.

Code is in project [VotingSystem.Persistence.Dapper](src/VotingSystem.Persistence.Dapper).



## Todo list:
- write a decent readme,
- EF Core persistence,
- configuration module.