// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Attributes;

namespace CliArgsParser.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a class that contains force arguments.
/// </summary>
public class ForceArgs : NoArgs {
    /// <summary>
    /// Represents a boolean flag that can be used to force certain actions.
    /// </summary>
    /// <remarks>
    /// The <c>Force</c> property is used to indicate whether a force action should be performed or not.
    /// When set to <c>true</c>, the action will be forced, while setting it to <c>false</c> will not force the action.
    /// </remarks>
    [ArgFlag('f', "force", "Let's force some stuff")]
    public bool Force { get; set; } = false;
}