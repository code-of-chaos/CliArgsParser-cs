// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Example.ArgsStandalone;
using Microsoft.Extensions.DependencyInjection;

namespace CliArgsParser.ExampleArgs;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
internal static class Program {
    public async static Task Main(string[] args) {
        IServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCliArgsParser(configuration =>
            configuration
                .SetConfig(new CliArgsParserConfig {
                    Overridable = true,
                    GenerateShortNames = true,
                    EnableExitAtlas = false
                })
                .AddFromAssembly(typeof(Program).Assembly)
                .AddFromAssembly(typeof(HelloAtlas).Assembly)
        );
        
        ServiceProvider provider = serviceCollection.BuildServiceProvider();
        var parser =  provider.GetRequiredService<IArgsParser>();

        await parser.ParseAsyncLinear("hello-args --username=Andreas");
        await parser.ParseAsyncLinear("help");
        await parser.ParseAsyncLinear("""help --name="hello-args" """);
        await parser.ParseAsyncLinear("help --expand");
        
        await parser.ParseAsyncLinear(args);
    }
}
