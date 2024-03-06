// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CliArgsParser;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents the state of the command execution output.
/// </summary>
[Flags]
public enum OutputState {
    /// <summary>
    /// Represents the state of the command execution output.
    /// </summary>
    False = 1,

    /// <summary>
    /// Represents the state of the command execution output.
    /// </summary>
    True = 2,

    /// <summary>
    /// Enum member representing an undefined state of the command execution output.
    /// </summary>
    Undefined = 4
} 