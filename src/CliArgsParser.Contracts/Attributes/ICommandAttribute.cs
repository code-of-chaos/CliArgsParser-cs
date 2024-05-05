// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace CliArgsParser.Contracts.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents an attribute that defines a command.
/// </summary>
public interface ICommandAttribute {
    /// <summary>
    /// Gets the name of the command.
    /// </summary>
    /// <remarks>
    /// This property is used to specify the name of the command represented by the <see cref="CommandAttribute"/>.
    /// </remarks>
    string Name { get; }

    /// <summary>
    /// Represents an attribute for a command.
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Represents the type of command line arguments that the command accepts.
    /// </summary>
    Type ArgsType { get; }
}