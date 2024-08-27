// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.ExampleData;

namespace CliArgsParser.Example.CliStandalone;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
internal static class Program {
    public async static Task Main(string[] _) {
        ICliParser parser = CliParser.CreateStandalone(
            configuration =>
                configuration
                    .SetConfig(new CliArgsParserConfig {
                        Overridable = true,
                        GenerateShortNames = true
                    })
                    .AddFromAssembly(typeof(Program).Assembly)
                    .AddFromAssembly(typeof(HelloAtlas).Assembly)
        );
        await parser.StartParsingAsync();
    }
}
