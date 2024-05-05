// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace CliArgsParser.Contracts.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a Value argument used in command line argument parsing.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public abstract class ArgValue(string shortName, string longName, string? description) : ArgAttribute(shortName, longName, description);