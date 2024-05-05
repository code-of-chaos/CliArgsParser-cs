// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CliArgsParser.Contracts;
using CliArgsParser.Contracts.Attributes;
using CliArgsParser.PreMade.Args;

namespace CliArgsParser.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

/// <summary>
/// Represents an attribute that marks a method as a command.
/// </summary>
/// <seealso cref="System.Attribute" />
/// <seealso cref="CliArgsParser.Contracts.Attributes.ICommandAttribute" />
[AttributeUsage(AttributeTargets.Method)]
public class CommandAttribute<T>(string name, string? description = null) : Attribute, ICommandAttribute where T : IParameters, new() {
    /// <summary>
    /// Represents a command attribute that can be applied to a method.
    /// </summary>
    /// <typeparam name="T">The type of the command's parameters.</typeparam>
    /// <remarks>
    /// This attribute is used to mark a method as a command that can be invoked by the command-line parser.
    /// </remarks>
    public string Name { get; } = name;

    /// <summary>
    /// Represents an attribute indicating a command.
    /// </summary>
    /// <typeparam name="T">The type of parameters for the command.</typeparam>
    public string? Description { get; } = description;

    /// <summary>
    /// Represents the type of the arguments for a command.
    /// </summary>
    public Type ArgsType { get; } = typeof(T);
}

/// <summary>
/// Represents an attribute that marks a method as a command.
/// </summary>
/// <typeparam name="T">The type of the parameters for the command.</typeparam>
[AttributeUsage(AttributeTargets.Method)]
public class CommandAttribute(string name, string? description = null) : CommandAttribute<NoArgs>(name, description);