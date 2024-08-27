// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CliArgsParser.Contracts.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// When used as an attribute on a property in a <see cref="ICommandParameters"/> class or on a Command method on a <see cref="ICommandAtlas"/>,
/// defines the description to that CLI element. Commonly used by the <c>help</c> command to display helpfull information.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
public class DescriptionAttribute(string description) : Attribute {
    /// <summary>
    /// The description string.
    /// </summary>
    public string Description { get; } = description;
}
