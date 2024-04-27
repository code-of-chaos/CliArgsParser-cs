// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CliArgsParser.Contracts.Attributes;
using CliArgsParser.PreMade.Args;

namespace CliArgsParser.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

[AttributeUsage(AttributeTargets.Method)]
public class CommandAttribute<T>(string name, string? description = null) : Attribute, ICommandAttribute where T : notnull, new() {
    public string Name { get; } = name;
    public string? Description { get; } = description;
    public Type ArgsType { get; } = typeof(T);
}

[AttributeUsage(AttributeTargets.Method)]
public class CommandAttribute(string name, string? description = null) : CommandAttribute<NoArgs>(name, description);