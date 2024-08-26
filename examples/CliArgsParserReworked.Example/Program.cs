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
    public static void Main(string[] args) {
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
        var parser =  provider.GetRequiredService<CliArgsParser>();

        parser.Execute(["hello"]);
        parser.Execute(["test-args"]);
    }
}
