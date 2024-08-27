// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CliArgsParser.Contracts.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// When used as an attribute on a property in a <see cref="ICommandParameters"/> class,
/// defines a command argument as a boolean flag.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ArgFlagAttribute(string name, string? shortName = null) : Attribute, IAttributeWithName {
    /// <inheritdoc cref="IAttributeWithName.Name"/>
    public string Name { get; } = name;
    
    /// <inheritdoc cref="IAttributeWithName.ShortName"/>
    public string? ShortName { get; } = shortName;
}
