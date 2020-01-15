using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MrgInfo.AdoQuery.Sql;

namespace MrgInfo.AdoQuery.App
{
    [SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
    [SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "InterpolatedStringExpressionIsNotIFormattable")]
    static class Examples
    {
        static void AdoExample()
        {
            const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;User Id=AdoQuery;Password=AdoQuery;";
            using DbConnection connection = new SqlConnection(connectionString);
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

        [SuppressMessage("ReSharper", "SuggestVarOrType_Elsewhere")]
        static void Example()
        {
            const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;User Id=AdoQuery;Password=AdoQuery;";
            const string prefix = "A";

            var provider = new SqlQueryProvider(connectionString);
            var resultSet = provider.Query<(int, string)>($@"
                |  select ProductId,
                |         Name
                |    from Product
                |   where Code like {prefix:=*}
                |order by ProductId");
            foreach ((int productId, string name) in resultSet)
            {
                Trace.WriteLine($"ProductId = {productId}, Name = {name}");
            }
        }

        static async Task AdoExampleAsync(CancellationToken cancellationToken = default)
        {
            const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;User Id=AdoQuery;Password=AdoQuery;";
            await using DbConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync(cancellationToken);
            await using DbCommand command = connection.CreateCommand();
            command.CommandText = @"
                select ProductId,
                       Name
                  from Product
                 where Code like @Prefix
              order by ProductId";
            command.Parameters.Add(new SqlParameter("Prefix", SqlDbType.NVarChar, 100) { Value = "A%" });
            await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                (int productId, string name) = (
                    await reader.GetFieldValueAsync<int>(0, cancellationToken),
                    await reader.IsDBNullAsync(1, cancellationToken)
                        ? ""
                        : await reader.GetFieldValueAsync<string>(1, cancellationToken));
                Trace.WriteLine($"ProductId = {productId}, Name = {name}");
            }
        }

        [SuppressMessage("ReSharper", "SuggestVarOrType_Elsewhere")]
        static async Task ExampleAsync(CancellationToken cancellationToken = default)
        {
            const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;User Id=AdoQuery;Password=AdoQuery;";
            const string prefix = "A";

            var provider = new SqlQueryProvider(connectionString);
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
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles")]
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        static async Task Main()
        {
            AdoExample();
            Example();
            await AdoExampleAsync();
            await ExampleAsync();
        }
    }
}
