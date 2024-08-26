// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CliArgsParserReworked.Contracts.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Property)]
public class ArgValueAttribute(string name, string? shortName = null) : Attribute, IAttruteWithName {
    public string Name { get; } = name;
    public string? ShortName { get; } = shortName;
}
