// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CliArgsParser.Contracts;

namespace CliArgsParser.PreMade.Parsers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CliParser(bool breakOnError = true) : AbstractParser(breakOnError) {
    public override void TryParse(string input) {
        throw new NotImplementedException();
    }

    public override Task TryParseAsync(string input) {
        throw new NotImplementedException();
    }
}