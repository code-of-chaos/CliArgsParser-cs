// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CliArgsParser.Tests.Data.Commands;
using CliArgsParser.Tests.Data.Parsers;

namespace CliArgsParser.Tests.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EmptyParserFixture : IDisposable {
    public readonly IParser Parser = new ParserConfiguration()
        .RegisterAtlas(new CommandAtlas())
        .CreateEmptyParser();
    
    public void Dispose() { }
}