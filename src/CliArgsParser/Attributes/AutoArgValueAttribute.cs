// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CliArgsParser.Contracts.Attributes;

namespace CliArgsParser.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents an attribute that can be applied to properties in a class to define command line argument values.
/// These attributes always return string, or from string castable values
/// The Short handle for this command is automatically generated.
/// Warning: If commands have the same initials, this will result in issues. 
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class AutoArgValueAttribute(string longName, string? description = null)
    : ArgValue(string.Join("", longName.Split('-').Select(t => t.First())), longName, description);
    