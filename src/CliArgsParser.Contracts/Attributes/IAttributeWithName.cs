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
/// All properties defined in a <see cref="ICommandParameters"/> class should have attributes which derive from this interface.
/// Used for name assignment of an argument of a command.
/// </summary>
public interface IAttributeWithName {
    /// <summary>
    /// Sets the name of the command argument
    /// </summary>
    string Name { get; }
    /// <summary>
    /// Sets a pre-determined short name (usually a single letter) for the command argument
    /// </summary>
    string? ShortName { get; }
}
