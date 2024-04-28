// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CliArgsParser.Contracts;
using Serilog;

namespace CliArgsParser.Examples.RegisterAtlas;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
static class Program {
    public static async Task MainAsync(IParser parser, string[] args) {
        await parser.TryParseAsync("hello");
        await parser.TryParseAsync("hello-async");
        await parser.TryParseAsync("""hello-test --username="andreas" """);
        await parser.TryParseAsync("hello-test-async --username=andreas");
    }
    
    public static void Main(string[] args) {
        ILogger logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();

        IParser parser = new ParserConfiguration()
            .SetLogger(logger)
            .RegisterAtlas(new HelloAtlas())
            .CreateArgsParser();

        MainAsync(parser, args).GetAwaiter().GetResult();
    }
}