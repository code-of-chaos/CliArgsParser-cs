// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace CliArgsParser;
// ReSharper restore CheckNamespace

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The `CliArgsParserConfig` struct represents the configuration options for the `CliArgsParser` class.
/// </summary>
public readonly struct CliArgsParserConfig() {
    /// <summary>
    /// These types are registered in the service provider and used to create command atlas instances dynamically during runtime.
    /// The command atlas types define the behavior and functionality of each command.
    /// </summary>
    public List<Type> CommandAtlasTypes { get; init; } = [];
    /// <summary>
    /// The <see cref="CommandParameterTypes"/> property is a <see cref="HashSet{T}"/> of <see cref="Type"/> that represents the types of command parameter classes.
    /// Command parameter classes are registered in the service provider to be used with the CLI argument parser.
    /// </summary>
    public HashSet<Type> CommandParameterTypes { get; init; } = [];
    /// <summary>
    /// Gets a value indicating whether this configuration is overridable.
    /// If set to true, the configuration can be overridden or extended by other configurations.
    /// If set to false, the configuration cannot be overridden or extended.
    /// The default value is false.
    /// </summary>
    public bool Overridable { get; init; } = true;
    /// <summary>
    /// Determines whether short names should be generated for commands.
    /// </summary>
    public bool GenerateShortNames { get; init; } = true;
    /// <summary>
    /// The lifetime of service instances for the command atlases registered in the service provider.
    /// </summary>
    public ServiceLifetime AtlasesServiceLifetime { get; init; } = ServiceLifetime.Transient;
    /// <summary>
    /// Represents the cursor symbol used in the command line interface (CLI).
    /// </summary>
    public string CliCursor { get; init; } = "> : ";

    /// <summary>
    /// Gets or sets a value indicating whether the help command atlas is enabled.
    /// </summary>
    /// <value>
    /// <c>true</c> if the atlas is enabled; otherwise, <c>false</c>.
    /// </value>
    public bool EnableHelpAtlas { get; init; } = true;
    /// <summary>
    /// Gets or sets a value indicating whether the exit command atlas is enabled.
    /// </summary>
    /// <value>
    /// <c>true</c> if the atlas is enabled; otherwise, <c>false</c>.
    /// </value>
    public bool EnableExitAtlas { get; init; } = true;
}
