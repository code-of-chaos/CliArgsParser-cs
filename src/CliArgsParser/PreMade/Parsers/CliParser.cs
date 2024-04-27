// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CliArgsParser.Contracts;

namespace CliArgsParser.PreMade.Parsers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CliParser: AbstractParser {
    public override bool TryParse<T>(string input, out T? output) where T : default {
        throw new NotImplementedException();
    }

    public override Task<bool> TryParseAsync<T>(string input, out T? output) where T : default {
        throw new NotImplementedException();
    }
}