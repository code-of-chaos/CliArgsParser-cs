// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParserReworked.Contracts.Types;
using Microsoft.Extensions.DependencyInjection;

namespace CliArgsParserReworked.Example;
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
                .AddFromType<HelpAtlas>()
                .AddFromType<HelloAtlas>()
        );
        
        ServiceProvider provider = serviceCollection.BuildServiceProvider();
        var parser =  provider.GetRequiredService<CliArgsParser>();

        await parser.ExecuteAsync(string.Join(" ", ["hello-args", "--username=Andreas"]));
        await parser.ExecuteAsync(string.Join(" ", ["help"]));
        await parser.ExecuteAsync(string.Join(" ", ["help", """--name="hello-args" """]));
        await parser.ExecuteAsync(string.Join(" ", ["help", "--expand"]));
        // parser.Execute(["hello-args", "--username=Andreas"]);
    }
}
