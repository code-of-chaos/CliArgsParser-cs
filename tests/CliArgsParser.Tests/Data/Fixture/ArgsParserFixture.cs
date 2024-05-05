// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CliArgsParser.Tests.Data.Commands;
using static System.GC;

namespace CliArgsParser.Tests.Data.Fixture;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ArgsParserFixture : IDisposable {
    public DataOutput DataOutput { get; private set; }
    public IParser Parser { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ArgsParserFixture() {
        DataOutput = new DataOutput();
        Parser = new ParserConfiguration()
            .RegisterAtlas(new CommandAtlas(DataOutput))
            .CreateArgsParser();
    }

    public void Dispose() {
        DataOutput = new DataOutput();
        SuppressFinalize(this);
    }

    public void ResetData() {
        DataOutput = new DataOutput();
    }
}