// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;

// ReSharper disable CheckNamespace
namespace CliArgsParser;
// ReSharper restore CheckNamespace


// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents the configuration for a CliArgsParser.
/// </summary>
public interface ICliArgsParserConfiguration {
    /// <summary>
    /// Sets the configuration for the CliArgsParser.
    /// </summary>
    /// <param name="config">The configuration to set.</param>
    /// <returns>The current ICliArgsParserConfiguration instance.</returns>
    ICliArgsParserConfiguration SetConfig(CliArgsParserConfig config);
    
    /// <summary>
    /// Adds the command atlas and command parameter types from the specified assembly to the parser configuration.
    /// </summary>
    /// <param name="assembly">The assembly from which to add the command atlas and command parameter types.</param>
    /// <returns>The instance of the ICliArgsParserConfiguration with the updated configuration.</returns>
    ICliArgsParserConfiguration AddFromAssembly(Assembly assembly);
    
    /// <summary>
    /// Adds a command type to the configuration.
    /// </summary>
    /// <typeparam name="T">The command type to add.</typeparam>
    /// <returns>The updated <see cref="ICliArgsParserConfiguration"/> instance.</returns>
    ICliArgsParserConfiguration AddFromType<T>() where T : ICommandAtlas;
}
