// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.ExampleData;
using Microsoft.Extensions.DependencyInjection;

namespace CliArgsParser.ExampleCli;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
internal static class Program {
    public async static Task Main(string[] _) {
        IServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCliArgsParser(configuration =>
            configuration
                .SetConfig(new CliArgsParserConfig {
                    Overridable = true,
                    GenerateShortNames = true
                })
                .AddFromType<HelloAtlas>()
        );

        ServiceProvider provider = serviceCollection.BuildServiceProvider();
        var cliParser = provider.GetRequiredService<ICliParser>();

        await cliParser.StartParsingAsync();
    }
}
