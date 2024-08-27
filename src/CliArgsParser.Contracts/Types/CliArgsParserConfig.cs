// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

namespace CliArgsParser.Contracts.Types;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct CliArgsParserConfig(bool overridable = false, bool generateShortNames = true) {
    public List<Type> CommandAtlasTypes { get; } =  [];
    public HashSet<Type> CommandParameterTypes { get; } = [];
    public bool Overridable { get; } = overridable;
    public bool GenerateShortNames { get; } = generateShortNames;
    public ServiceLifetime AtlasesServiceLifetime { get; } = ServiceLifetime.Transient;
    public string CliCursor { get; } = "> : ";
    public bool EnableHelpAtlas { get; } = true;
    public bool EnableExitAtlas { get; } = true;
}
