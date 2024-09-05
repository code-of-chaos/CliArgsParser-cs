// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.ExampleData;

namespace CliArgsParser.Example.HeadlessAllowArgs;
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
                        EnableExitAtlas = false, // Ensure this is disabled for ArgsParsers.
                        HeadlessMode = HeadlessTypes.AllowInputArguments,
                        HeadlessModeCommand = "hello-args" // insert full string command, with arguments, it should execute if the `args` is empty
                    })
                    .AddFromAssembly(typeof(Program).Assembly)
                    .AddFromAssembly(typeof(HelloAtlas).Assembly)
        );
        await parser.ParseAsyncLinear("--username=\"hello-args\"");
    }
}
