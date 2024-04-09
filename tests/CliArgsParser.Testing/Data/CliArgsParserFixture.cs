// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CliArgsParser.Testing.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CliArgsParserFixture : IDisposable {
    public readonly ICliArgsParser Parser = new CliArgsParser()
        .RegisterFromCliAtlas(new CommandAtlas())
        .RegisterFromCliAtlas(new CommandAtlasAsync());
    public void Dispose() { }
}