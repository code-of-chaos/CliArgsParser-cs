// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Tests.Data.Commands;
using Microsoft.Extensions.DependencyInjection;
using static System.GC;

namespace CliArgsParser.Tests.Data.Fixture;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ArgsParserHeadlessAsyncFixture : IDisposable {
    public DataOutput DataOutput { get; private set; }
    public IArgsParser Parser { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ArgsParserHeadlessAsyncFixture() {
        DataOutput = new DataOutput();
        IServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton(DataOutput);

        serviceCollection.AddCliArgsParser(configuration =>
            configuration
                .SetConfig(new CliArgsParserConfig {
                    Overridable = true,
                    GenerateShortNames = true,
                    HeadlessMode = HeadlessTypes.AllowInputArguments,
                    HeadlessModeCommand = "test-sync-params-empty"
                })
                .AddFromType<CommandAtlas>()
        );

        ServiceProvider provider = serviceCollection.BuildServiceProvider();
        Parser = provider.GetRequiredService<IArgsParser>();
    }

    public void Dispose() {
        DataOutput = new DataOutput();
        SuppressFinalize(this);
    }

    public void ResetData() {
        DataOutput = new DataOutput();
    }
}
