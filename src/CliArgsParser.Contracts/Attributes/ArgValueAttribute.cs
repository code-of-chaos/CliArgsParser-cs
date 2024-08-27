// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CliArgsParser.Contracts.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Property)]
public class ArgValueAttribute(string name, string? shortName = null) : Attribute, IAttributeWithName {
    public string Name { get; } = name;
    public string? ShortName { get; } = shortName;
}
