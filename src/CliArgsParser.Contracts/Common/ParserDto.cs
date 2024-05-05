// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Serilog;

namespace CliArgsParser.Contracts.Common;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a DTO (Data Transfer Object) for the parser setup information.
/// </summary>
public record ParserDto(
    ILogger Logger,
    bool HasAsyncCommands,
    Dictionary<string, CommandRecord> CommandStructs
);
