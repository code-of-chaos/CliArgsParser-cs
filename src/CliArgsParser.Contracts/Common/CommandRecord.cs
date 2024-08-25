// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace CliArgsParser.Contracts.Common;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

/// <summary>
/// Represents a command record containing properties related to a command.
/// </summary>
public record CommandRecord(
    string Name,
    string? Description,
    Delegate Delegate,
    Type ReturnType,
    bool IsAsync,
    IParameterParser ParameterParser
);
