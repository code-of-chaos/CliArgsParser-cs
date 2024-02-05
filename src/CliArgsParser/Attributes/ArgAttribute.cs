// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Contracts.Attributes;

namespace CliArgsParser.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class ArgAttribute(char shortName, string longName, string? description) : Attribute, IArgAttribute {
    public char ShortName { get; } = shortName;
    public string LongName { get; } = longName.Replace("-", "");
    public string? Description { get; } = description;
}