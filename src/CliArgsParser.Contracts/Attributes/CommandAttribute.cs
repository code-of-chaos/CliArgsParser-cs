// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CliArgsParser.Contracts.Types;

namespace CliArgsParser.Contracts.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents an attribute that marks a method as a command.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class CommandAttribute(string? name = null, Type? argsType = null) : Attribute {
    /// <summary>
    /// Represents a command attribute that can be applied to a method.
    /// </summary>
    public string? Name { get; } = name;

    /// <summary>
    /// Represents the type of the arguments for a command.
    /// </summary>
    public Type ArgsType { get; } = argsType ?? typeof(NoArgs);
}
/// <summary>
/// Represents an attribute that marks a method as a command.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class CommandAttribute<T>(string? name = null) : CommandAttribute(name, typeof(T)) where T : ICommandParameters, new();
