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
    public string Name { get; } = name;

    /// <summary>
    /// Represents an attribute indicating a command.
    /// </summary>
    public string? Description { get; } = description;

    /// <summary>
    /// Represents the type of the arguments for a command.
    /// </summary>
    public Type ArgsType { get; } = typeof(T);
}

/// <summary>
/// Represents an attribute that marks a method as a command.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class CommandAttribute(string name, string? description = null) : CommandAttribute<NoArgs>(name, description);
