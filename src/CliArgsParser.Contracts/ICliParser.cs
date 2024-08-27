// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable CheckNamespace
namespace CliArgsParser;
// ReSharper restore CheckNamespace


// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The CliParser class is responsible for parsing command line input and executing corresponding commands.
/// </summary>
public interface ICliParser {
    /// <summary>
    /// Used for the keeping alive of the <see cref="StartParsing"/> and <see cref="StartParsingAsync"/> methods
    /// </summary>
    bool IsAlive { get; set; }
    
    /// <summary>
    /// Starts parsing the command line input.
    /// </summary>
    /// <remarks>
    /// This method reads the command line input from the user and executes the corresponding commands.
    /// It continues to read user input until the <see cref="IsAlive"/> property is set to false.
    /// For each command string, the <see cref="ICliArgsParser.Execute"/> method is called to execute the command.
    /// </remarks>
    void StartParsing();
    
    /// <summary>
    /// Asynchronously starts parsing the command line input.
    /// </summary>
    /// <remarks>
    /// This method reads the command line input from the user and executes the corresponding commands.
    /// It continues to read user input until the <see cref="IsAlive"/> property is set to false.
    /// For each command string, the <see cref="ICliArgsParser.ExecuteAsync"/> method is called to execute the command.
    /// </remarks>
    Task StartParsingAsync();
}
