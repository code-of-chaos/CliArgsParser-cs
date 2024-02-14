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
    ICliArgsParser RegisterFromDlLs(IEnumerable<string> filePaths, Action? assignedCallback = null);

    bool[] TryParseMultiple(IEnumerable<string> args);
    bool TryParse(IEnumerable<string> args);

    void TryParseInput(bool breakOnFalse = false, bool allowMultiple = false);
}