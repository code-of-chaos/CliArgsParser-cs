// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CliArgsParser.Contracts.Attributes;

namespace CliArgsParser.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents an attribute that is used to mark a property as a boolean argument in command line argument parsing.
/// These types of attributes always return a bool.
/// The Short handle for this command is automatically generated.
/// Warning: If commands have the same initials, this will result in issues. 
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class AutoArgFlagAttribute(string longName, string? description = null)
    : ArgFlag(string.Join("", longName.Split('-').Select(t => t.First())), longName, description);
