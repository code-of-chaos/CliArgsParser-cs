// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CliArgsParser.Contracts;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents the interface for parameter options.
/// </summary>
public interface IParameterOptions {
    /// <summary>
    /// Gets or sets a value indicating whether the program should be verbose.
    /// </summary>
    public bool Verbose { get; set; }
}