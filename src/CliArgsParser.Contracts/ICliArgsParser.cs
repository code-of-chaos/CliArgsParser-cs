// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CliArgsParser.Contracts;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents the interface for parsing command-line arguments.
/// </summary>
public interface ICliArgsParser {
    /// <summary>
    /// Registers commands from a CLI command atlas.
    /// </summary>
    /// <typeparam name="T">The type of CLI command atlas.</typeparam>
    /// <param name="cliCommandAtlas">The CLI command atlas.</param>
    /// <param name="force">Flag indicating whether to overwrite existing commands with the same name.</param>
    /// <returns>The instance of the ICliArgsParser interface.</returns>
    ICliArgsParser RegisterFromCliAtlas<T>(T cliCommandAtlas, bool force = false) where T : ICliCommandAtlas;

    /// <summary>
    /// Registers the commands from the CliAtlas into the CliArgsParser.
    /// </summary>
    /// <typeparam name="T">The type of the CliCommandAtlas.</typeparam>
    /// <param name="cliCommandAtlas">The CliCommandAtlas containing the commands to register.</param>
    /// <param name="force">A flag indicating whether to overwrite existing commands with the same name.</param>
    /// <returns>The instance of the CliArgsParser.</returns>
    ICliArgsParser RegisterFromCliAtlas<T>(IEnumerable<T> cliCommandAtlas, bool force = false) where T : ICliCommandAtlas;

    /// <summary>
    /// Registers CLI commands from DLL files.
    /// </summary>
    /// <param name="filePaths">An IEnumerable of file paths of the DLL files.</param>
    /// <param name="assignedCallback">An optional callback action to be invoked after each command is registered.</param>
    /// <returns>The instance of the <see cref="CliArgsParser"/> class to allow method chaining.</returns>
    ICliArgsParser RegisterFromDlLs(IEnumerable<string> filePaths, Action? assignedCallback = null);

    /// <summary>
    /// Parses multiple command line arguments and returns a list of boolean values indicating the success of each command.
    /// </summary>
    /// <param name="args">The command line arguments to parse.</param>
    /// <returns>A list of boolean values indicating the success of each command.</returns>
    bool[] TryParseMultiple(IEnumerable<string> args);

    /// <summary>
    /// Tries to parse the command line arguments.
    /// </summary>
    /// <param name="args">The command line arguments to parse.</param>
    /// <returns>True if the parsing was successful, otherwise false.</returns>
    bool TryParse(IEnumerable<string> args);

    /// Tries to parse the input arguments.
    /// @param breakOnFalse Specifies whether to break the parsing loop if a false result is encountered. Default is false.
    /// @param allowMultiple Specifies whether to allow multiple sets of input arguments. Default is false.
    void TryParseInput(bool breakOnFalse = false, bool allowMultiple = false);
}