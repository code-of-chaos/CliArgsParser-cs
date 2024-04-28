// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CliArgsParser.Tests.Data.Commands;

namespace CliArgsParser.Tests.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ArgsParserFixture : IDisposable {
    public readonly IParser Parser = new ParserConfiguration()
        .RegisterAtlas(new CommandAtlas())
        .CreateArgsParser();
    
    public void Dispose() { }
}