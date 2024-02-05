// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CliArgsParser.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Property)]
public class ArgValueAttribute(char shortName, string longName, string? description = null) : ArgAttribute(shortName, longName, description) {
    // public T AttributeType { get; }
}
    