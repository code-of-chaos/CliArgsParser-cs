// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Commands;
using CliArgsParser.Contracts;
using CliArgsParser.Contracts.Attributes;

namespace CliArgsParser.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Attribute used to mark a method as a CLI command.
/// </summary>
/// <typeparam name="T">The type of the parameter options.</typeparam>
/// <param name="name">The name of the CLI command.</param>
/// <param name="description">The description of the CLI command.</param>
/// <remarks>
/// This attribute should be applied to methods that serve as CLI command entry points.
/// </remarks>
[AttributeUsage(AttributeTargets.Method)]
public class CliCommandAttribute<T>(string name, string? description = null) : Attribute, ICliCommandAttribute where T: IParameterOptions , new() {
    public string Name { get; } = name;
    public string? Description { get; } = description;

    /// <summary>
    /// Represents the type of the parameter options for a CLI command.
    /// </summary>
    public Type ParameterOptionsType { get; } = typeof(T);
    
    private readonly ParameterParser<T> _parameterParser = new();

    /// <summary>
    /// Retrieves the parameter options for a CLI command.
    /// </summary>
    /// <typeparam name="T">The type of parameter options.</typeparam>
    /// <param name="args">The command line arguments.</param>
    /// <returns>The parameter options parsed from the command line arguments.</returns>
    public IParameterOptions GetParameters(IEnumerable<string> args) => _parameterParser.Parse(args);
}

/// <summary>
/// Represents a default CLI command which doesn't take in any arguments.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class CliCommandAttribute(string name, string? description = null)
    : CliCommandAttribute<NoArgs>(name, description);

