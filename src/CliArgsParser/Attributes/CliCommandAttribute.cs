// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Contracts;
using CliArgsParser.Contracts.Attributes;

namespace CliArgsParser.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Method)]
public class CliCommandAttribute<T>(string name, string? description = null) : Attribute, ICliCommandAttribute where T: IParameterOptions , new() {
    public string Name { get; } = name;
    public string? Description { get; } = description;
    public Type ParameterOptionsType { get; } = typeof(T);
    private readonly ParameterParser<T> _parameterParser = new();
    public IParameterOptions GetParameters(IEnumerable<string> args) => _parameterParser.Parse(args);
}
