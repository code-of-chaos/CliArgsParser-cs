// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Contracts;

namespace CliArgsParser.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a class that contains no arguments.
/// </summary>
public class NoArgs : IParameterOptions{
    /// <summary>
    /// Gets or sets the verbosity level.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The verbosity level determines the amount of detail or information the program outputs.
    /// When set to <c>true</c>, the program will output more detailed information.
    /// </para>
    /// </remarks>
    public bool Verbose { get; set; }
}