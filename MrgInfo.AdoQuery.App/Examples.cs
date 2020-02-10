using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MrgInfo.AdoQuery.Core;
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
        }

        [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
        static void FakeExample()
        {
            var provider = new ByPatternFakeQueryProvider
            {
                ["ProductId.+Code.+Product"] = new[]
                {
                    new object[] { 10, "AB123", "Leather Sofa", 1000.0 },
                    new object[] { 20, "AB456", "Baby Chair", 200.25 },
                    new object[] { 30, "AB789", "Sport Shoes", 250.60 },
                    new object[] { 40, "PQ123", "Sony Digital Camera", 399 },
                    new object[] { 50, "PQ456", "Hitachi HandyCam", 1050.0 },
                    new object[] { 60, "PQ789", "GM Saturn", 2250.99 },
                }
            };
            (int productId, string code) = provider
                .Query<int, string>($@"
                    |select ProductId,
                    |       Code
                    |  from Product")
                .First();
            Trace.WriteLine($"ProductId = {productId}, Code = {code}");
        }

        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope")]
        static async Task AdoExampleAsync(CancellationToken cancellationToken = default)
        {
            await using DbConnection connection = new OracleConnection("Data Source=localhost:1521/ORCLCDB.localdomain;User Id=adoquery;Password=adoquery;");
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            await using var command = connection.CreateCommand();
            command.CommandText = @"
                select ProductId,
                       Name
                  from Product
                 where Code like :Prefix
              order by ProductId";
            command.Parameters.Add(new OracleParameter("Prefix", OracleDbType.Varchar2, 100) { Value = "A%" });
            await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                (int productId, string name) = (
                    (int)await reader.GetFieldValueAsync<decimal>(0, cancellationToken).ConfigureAwait(false),
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
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles")]
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        static async Task Main()
        {
            AdoExample();
            Example();
            FakeExample();
            await AdoExampleAsync().ConfigureAwait(false);
            await ExampleAsync().ConfigureAwait(false);
        }
    }
}
