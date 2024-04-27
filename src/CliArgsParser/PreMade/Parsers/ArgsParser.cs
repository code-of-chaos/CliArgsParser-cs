// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CliArgsParser.Contracts.Common;

namespace CliArgsParser.PreMade.Parsers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ArgsParser : AbstractParser {
    public override bool TryParse<T>(string input, out T? output) where T : default {
        output = default;

        foreach (string commandString in GetCommands(input)) {
            if (!TryGetCommand(commandString, out string? commandName, out Dictionary<string, string>? args)) {
                return false;
            }

            if (!CommandStructs.TryGetValue(commandName, out CommandRecord? commandRecord)) {
                return false;
            }

            try {
                var parameters = commandRecord.ParameterParser.Parse(args);
                
                output = (T)commandRecord.Delegate.DynamicInvoke(parameters)!;
                return true;
            }
            catch {
                return false;
            }
        }

        return true;
    }

    public override Task<bool> TryParseAsync<T>(string input, out T? output) where T : default {
        throw new NotImplementedException();
    }
}