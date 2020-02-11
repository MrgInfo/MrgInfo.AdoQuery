using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using static MrgInfo.AdoQuery.App.AdoConsole;
using static MrgInfo.AdoQuery.App.Examples;

namespace MrgInfo.AdoQuery.App
{
    static class Program
    {
        [SuppressMessage("Style", "IDE1006:Naming Styles")]
        static async Task Main()
        {
            AdoExample();
            await AdoExampleAsync().ConfigureAwait(false);
            Example();
            await ExampleAsync().ConfigureAwait(false);
            MockPatternExample();
            MockIdExample();
            await RunAsync().ConfigureAwait(false);
        }
    }
}
