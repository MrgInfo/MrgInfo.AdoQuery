# MrgInfo.AdoQuery

Run safe and cosine ADO .NET SQL queries.

## Installation

For Oracle Dababase:

```powershell
PM> Install-Package MrgInfo.AdoQuery.Oracle
```

For Microsoft SQL Server:

```powershell
PM> Install-Package MrgInfo.AdoQuery.Sql
```

## Background

This is how you should write ADO queries filtered by input data:

```csharp
static void AdoExample()
{
    using DbConnection connection = new SqlConnection("Data Source=localhost;User Id=AdoQuery;Password=AdoQuery;");
    connection.Open();
    using DbCommand command = connection.CreateCommand();
    command.CommandText = @"
        select ProductId,
                Name
            from Product
            where Code like @Prefix
        order by ProductId";
    command.Parameters.Add(new SqlParameter("Prefix", SqlDbType.NVarChar, 100) { Value = "A%" });
    using DbDataReader reader = command.ExecuteReader();
    while (reader.Read())
    {
        (int productId, string name) = (reader.GetInt32(0), reader.IsDBNull(1) ? "" : reader.GetString(1));
        Trace.WriteLine($"ProductId = {productId}, Name = {name}");
    }
}
```

And this is how you always wanted:

```csharp
static void Example()
{
    var prefix = "A";
    var provider = new SqlQueryProvider("Data Source=localhost;User Id=AdoQuery;Password=AdoQuery;");
    var resultSet = provider.Query<(int, string)>($@"
        |  select ProductId,
        |         Name
        |    from Product
        |   where Code {prefix:=*}
        |order by ProductId");
    foreach ((int productId, string name) in resultSet)
    {
        Trace.WriteLine($"ProductId = {productId}, Name = {name}");
    }
}
```

> Separating SQL command from input data is a good idea becaused of both security and performance reasons.

## Can do `async` too

Fetching data from SQL server is generally I/O bound, asynchronous reading has a value here.

```csharp
var provider = new SqlQueryProvider("Data Source=(localdb)\\MSSQLLocalDB;User Id=AdoQuery;Password=AdoQuery;");
var prefix = "A";
var resultSet = provider.QueryAsync<(int, string)>($@"
    |  select ProductId,
    |         Name
    |    from Product
    |   where Code like {prefix:=*}
    |order by ProductId", cancellationToken);
await foreach ((int productId, string name) in resultSet.WithCancellation(cancellationToken))
{
    Trace.WriteLine($"ProductId = {productId}, Name = {name}");
}
```
