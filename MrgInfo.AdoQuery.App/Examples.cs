using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MrgInfo.AdoQuery.Oracle;
using MrgInfo.AdoQuery.Sql;
using Oracle.ManagedDataAccess.Client;

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

        [SuppressMessage("ReSharper", "SuggestVarOrType_Elsewhere")]
        [SuppressMessage("ReSharper", "ConvertToConstant.Local")]
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

        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope")]
        static async Task AdoExampleAsync(CancellationToken cancellationToken = default)
        {
            await using DbConnection connection = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SID=ORCLCDB)));User Id=adoquery;Password=adoquery;");
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            await using DbCommand command = connection.CreateCommand();
            command.CommandText = @"
                select ProductId,
                       Name
                  from Product
                 where Code like @Prefix
              order by ProductId";
            command.Parameters.Add(new SqlParameter("Prefix", SqlDbType.NVarChar, 100) { Value = "A%" });
            await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                (int productId, string name) = (
                    await reader.GetFieldValueAsync<int>(0, cancellationToken).ConfigureAwait(false),
                    await reader.IsDBNullAsync(1, cancellationToken).ConfigureAwait(false)
                        ? ""
                        : await reader.GetFieldValueAsync<string>(1, cancellationToken).ConfigureAwait(false));
                Trace.WriteLine($"ProductId = {productId}, Name = {name}");
            }
        }

        [SuppressMessage("ReSharper", "SuggestVarOrType_Elsewhere")]
        [SuppressMessage("ReSharper", "ConvertToConstant.Local")]
        static async Task ExampleAsync(CancellationToken cancellationToken = default)
        {
            var prefix = "A";
            var provider = new OracleQueryProvider("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SID=ORCLCDB)));User Id=adoquery;Password=adoquery;");
            var resultSet = provider.QueryAsync<(int, string)>($@"
                |  select ProductId,
                |         Name
                |    from Product
                |   where Code {prefix:=*}
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
            await AdoExampleAsync().ConfigureAwait(false);
            await ExampleAsync().ConfigureAwait(false);
        }
    }
}
