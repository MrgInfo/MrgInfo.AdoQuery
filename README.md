# MrgInfo.AdoQuery

Run safe and cosine ADO .NET SQL queries with this package.

## Installation

For Oracle Database:

```powershell
PM> Install-Package MrgInfo.AdoQuery.Oracle
```

For Microsoft SQL Server:

```powershell
PM> Install-Package MrgInfo.AdoQuery.Sql
```

## Background

This is how you should write ADO queries filtered by arbitrary input data:

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

And this is how you always dreamed:

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

Separating the SQL command from sensible input data is a good idea because of both **security** (SQL injection) and **performance** (better execution planning) reasons.

The ```QueryProvider``` uses string interpolation for adding user data for a particular query and administers **parameterized** SQL commands under the hood.

The result set can easily be enumerated and the projection part of the query simply transforms to named tuples. The requested types of tuple elements can be specified by generic type parameters of the query methods.

## Can run ```async``` too

Fetching data from SQL server is generally I/O bound asynchronous reading has a value here.

```csharp
var prefix = "A";
var provider = new OracleQueryProvider(new OracleConnectionStringBuilder
{
    DataSource = "Data Source=localhost:1521/ORCLCDB.localdomain",
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

When applying string interpolation one can use format strings. `QueryProvider` has some special
format string for dealing with scenarios when *NULL* and real data both can occur.

| Format          | Definition                    |
|-----------------|-------------------------------|
| `$"{data:==}"`  | Equals (works for NULLs).     |
| `$"{data:!=}"`  | Not equals (works for NULLs). |
| `$"{data:=*}"`  | Match start of string.        |
| `$"{data:*=}"`  | Match end of string.          |
| `$"{data:*=*}"` | Match containing in string.   |

## Mock data tables

Mock objects are simulated objects that mimic the behavior of real objects. It is possible to mock data table (more precisely complete query result sets) in order to
test business logic without an actual database server.

The ```MockByPatternQueryProvider``` can be used to provide mocked results to quires matching a given regular expression:

```csharp
var provider = new MockByPatternQueryProvider
{
    ["ProductId.+Code.+Product"] = new[]
    {
        new object?[] { 10, "AB123", "Leather Sofa", 1000.0 },
        new object?[] { 20, "AB456", "Baby Chair", 200.25 },
        new object?[] { 30, "AB789", "Sport Shoes", 250.60 },
        new object?[] { 40, "PQ123", "Sony Digital Camera", 399 },
        new object?[] { 50, "PQ456", "Hitachi HandyCam", 1050.0 },
        new object?[] { 60, "PQ789", "GM Saturn", 2250.99 },
    }
};
(int productId, string code) = provider
    .Query<int, string>($@"
        |select ProductId,
        |       Code
        |  from Product")
    .First();
Trace.WriteLine($"ProductId = {productId}, Code = {code}");
```

The ```MockByIdQueryProvider``` is useful for mocking quires embedded into the code with a unique identifier:

```csharp
var provider = new MockByIdQueryProvider
{
    ["42"] = new[]
    {
        new object?[] { 10, "AB123", "Leather Sofa", 1000.0 },
        new object?[] { 20, "AB456", "Baby Chair", 200.25 },
        new object?[] { 30, "AB789", "Sport Shoes", 250.60 },
        new object?[] { 40, "PQ123", "Sony Digital Camera", 399 },
        new object?[] { 50, "PQ456", "Hitachi HandyCam", 1050.0 },
        new object?[] { 60, "PQ789", "GM Saturn", 2250.99 },
    }
};
(int productId, string code) = provider
    .Query<int, string>(42.IdFor($@"
        |select ProductId,
        |       Code
        |  from Product"))
    .First();
Trace.WriteLine($"ProductId = {productId}, Code = {code}");
```

## Build

Status of the last build:

[![Build Status](https://wavezone.visualstudio.com/MrgInfo/_apis/build/status/Deploy%20package?branchName=master)](https://wavezone.visualstudio.com/MrgInfo/_build/latest?definitionId=4&branchName=master)

## About the author

![][avatar]|![][logo]{ width=50% }
:-:|:-:
Groma István Ph.D.|[MRG-Infó Bt.][home]



[home]: http://www.mrginfo.com/angol/index.html
[logo]: http://www.mrginfo.com/images/mrganim.png "MRG-Infó Bt."
[avatar]: https://www.gravatar.com/avatar/4bfd9bd9604131cd5c3eb25670d74475 "Groma István Ph.D."
