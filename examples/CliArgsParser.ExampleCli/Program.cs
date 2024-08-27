// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Contracts;
using CliArgsParser.Contracts.Types;
using Microsoft.Extensions.DependencyInjection;

namespace CliArgsParser.ExampleCli;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
internal static class Program {
    public async static Task Main(string[] args) {
        IServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCliArgsParser(configuration =>
            configuration
                .SetConfig(new CliArgsParserConfig(
                    overridable: true,
                    generateShortNames: true
                ))
                .AddFromType<HelloAtlas>()
        );
        
        ServiceProvider provider = serviceCollection.BuildServiceProvider();
        // var parser =  provider.GetRequiredService<IArgsParser>();

        // await parser.ParseAsync("hello-args --username=Andreas");
        // await parser.ParseAsync("help");
        // await parser.ParseAsync("""help --name="hello-args" """);
        // await parser.ParseAsync("help --expand");
        
        var cliParser =  provider.GetRequiredService<ICliParser>();
        await cliParser.StartParsingAsync();
    }
}
