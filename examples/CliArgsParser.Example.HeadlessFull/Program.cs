// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.ExampleData;

namespace CliArgsParser.Example.Headless;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
internal static class Program {
    public async static Task Main(string[] args) {
        IArgsParser parser = ArgsParser.CreateStandalone(
            configuration =>
                configuration
                    .SetConfig(new CliArgsParserConfig {
                        Overridable = true,
                        GenerateShortNames = true,
                        EnableExitAtlas = false, // Ensure this is disabled for ArgsParsers.
                        HeadlessMode = HeadlessTypes.IgnoreInputArguments,
                        HeadlessModeCommand = "hello" // insert full string command, with arguments, it should execute if the `args` is empty
                    })
                    .AddFromAssembly(typeof(Program).Assembly)
                    .AddFromAssembly(typeof(HelloAtlas).Assembly)
        );
        await parser.ParseAsyncLinear(args);
    }
}
