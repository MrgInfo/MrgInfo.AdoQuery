# MrgInfo.AdoQuery

Run safe and cosine ADO .NET SQL queries with this package.

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
```

And this is how you always wanted:

```csharp
var prefix = "A";
var provider = new SqlQueryProvider("Data Source=localhost;User Id=AdoQuery;Password=AdoQuery;");
var resultSet = provider.Query<int, string>($@"
    |  select ProductId,
    |         Name
    |    from Product
    |   where Code {prefix:=*}
    |order by ProductId");
foreach ((int productId, string name) in resultSet)
{
    Trace.WriteLine($"ProductId = {productId}, Name = {name}");
}
```

Separating SQL command from sensible input data is a good idea becaused of both **security** (SQL injection) and **performance** (better execution planning) reasons.

The ```QueryProvider``` uses string interpolation for addig user data for a particular query and administers **parameterized** SQL commands under the hood.

The result set can easally be enumerated and the projection part of the query simply tranforms to named tuples. The requeted types of tuple elements can be specified by generic type parameters of the query methods.

## Can run ```async``` too

Fetching data from SQL server is generally I/O bound asynchronous reading has a value here.

```csharp
var prefix = "A";
var provider = new OracleQueryProvider(new OracleConnectionStringBuilder
{
    DataSource = "(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SID=ORCLCDB)))",
    UserID = "adoquery",
    Password = "adoquery"
});
var resultSet = provider.QueryAsync<int, string>($@"
    |  select ProductId,
    |         Name
    |    from Product
    |   where Code {prefix:=*}
    |order by ProductId", cancellationToken);
await foreach ((int productId, string name) in resultSet.WithCancellation(cancellationToken))
{
    Trace.WriteLine($"ProductId = {productId}, Name = {name}");
}
```

## Format strings

When applying string interpolation one can use format strings. `QueryProvider` has some spetial
format string for dealing with scenarios when *NULL* and real data both can occur.

| Format           | Definition                    |
|------------------|-------------------------------|
| `$"{data:==}"`  | Equals (works for NULLs).     |
| `$"{data:!=}"`  | Not equals (works for NULLs). |
| `$"{data:=*}"`  | Match start of string.        |
| `$"{data:*=}"`  | Match end of string.          |
| `$"{data:*=*}"` | Match containing in string.   |

## Faking queries

