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
/// When used as an attribute on a property in a <see cref="ICommandParameters"/> class,
/// defines a command argument as a string value.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ArgValueAttribute(string name, string? shortName = null) : Attribute, IAttributeWithName {
    /// <inheritdoc cref="IAttributeWithName.Name"/>
    public string Name { get; } = name;
    
    /// <inheritdoc cref="IAttributeWithName.ShortName"/>
    public string? ShortName { get; } = shortName;
}
