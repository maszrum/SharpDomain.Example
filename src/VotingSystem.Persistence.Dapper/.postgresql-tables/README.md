## SQL queries to create tables

Here are files with SQL code to create tables in the database. It is executed automatically at the startup if the program detect that tables does not exist.

The files are named following the pattern: ``XX_tablename.sql`` where ``XX`` is the order of query execution.

These files are included to assembly as ``embedded resources``.

``VotingSystem.Persistence.Dapper.csproj``:
```xml
<ItemGroup>
    <EmbeddedResource Include=".postgresql-tables\XX_tablename.sql" />
</ItemGroup>
```