// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace CliArgsParser.Contracts.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a boolean Flag argument used in command line argument parsing.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public abstract class ArgFlag(string shortName, string longName, string? description) : ArgAttribute(shortName, longName, description);
