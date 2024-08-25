// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CliArgsParser.PreMade.Parsers;

namespace CliArgsParser.Tests.Data.Parsers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class EmptyParser(bool breakOnError = true) : AbstractParser(breakOnError) {
    public override void TryParse(string input) => throw new NotImplementedException();
    public override Task TryParseAsync(string input) => throw new NotImplementedException();

    // Making protected functions public
    public new Dictionary<string, string> GetArgs(string argsInput) => AbstractParser.GetArgs(argsInput);
    public new bool TryGetCommand(string input, out string? commandName, out Dictionary<string, string> args) => AbstractParser.TryGetCommand(input, out commandName, out args);
    public new IEnumerable<string> GetCommands(string input) => AbstractParser.GetCommands(input);
    public new void ProcessCommandString(string commandString) => base.ProcessCommandString(commandString);
    public new Task ProcessCommandStringAsync(string commandString) => base.ProcessCommandStringAsync(commandString);
}

public static class EmptyParserExtension {
    public static IParser CreateEmptyParser(this IParserConfiguration parserConfiguration, bool allowOverwrites = false) => new EmptyParser().IngestFromSetup(parserConfiguration.GetParserSetup());
}
