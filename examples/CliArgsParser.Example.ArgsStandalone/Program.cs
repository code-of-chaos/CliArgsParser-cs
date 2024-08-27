// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.ExampleData;

namespace CliArgsParser.Example.ArgsStandalone;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
internal static class Program {
    public async static Task Main(string[] _) {
        IArgsParser parser = ArgsParser.CreateStandalone(
            configuration =>
                configuration
                    .SetConfig(new CliArgsParserConfig {
                        Overridable = true,
                        GenerateShortNames = true,
                        EnableExitAtlas = false
                    })
                    .AddFromType<HelloAtlas>()
        );

        await parser.ParseAsyncLinear("hello-args --username=Andreas");
        await parser.ParseAsyncLinear("help");
        await parser.ParseAsyncLinear("""help --name="hello-args" """);
        await parser.ParseAsyncLinear("help --expand");
    }
}
