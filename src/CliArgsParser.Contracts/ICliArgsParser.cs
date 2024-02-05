// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CliArgsParser.Contracts;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICliArgsParser {
    ICliArgsParser RegisterFromCliAtlas<T>(T cliCommandAtlas, bool force = false) where T : ICliCommandAtlas;
    ICliArgsParser RegisterFromCliAtlas<T>(IEnumerable<T> cliCommandAtlas, bool force = false) where T : ICliCommandAtlas;
    ICliArgsParser RegisterFromDlLs(IEnumerable<string> filePaths);

    bool TryParse(IEnumerable<string> args, bool parseMutliple);
    bool TryParse(IEnumerable<string> args);
    
    void HelpCommand(string[] args);
}