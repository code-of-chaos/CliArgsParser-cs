// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using CliArgsParser.Contracts.Common;
using Serilog;

namespace CliArgsParser.Contracts;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

/// <summary>
/// Represents the configuration for a parser.
/// </summary>
public interface IParserConfiguration {
    /// <summary>
    /// Represents the logging property for the parser configuration.
    /// If not set during configuration, it is defined is Logger.None
    /// </summary>
    public ILogger Log { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Registers an atlas of type T in the parser configuration.
    /// </summary>
    /// <typeparam name="T">The type of the atlas to register. The type of T must be not null.</typeparam>
    /// <returns>The parser configuration after the atlas is registered.</returns>
    public IParserConfiguration RegisterAtlas<T>() where T : notnull;

    /// <summary>
    /// Registers an atlas type for parsing.
    /// </summary>
    /// <param name="t">The atlas type to register.</param>
    /// <returns>The updated parser configuration.</returns>
    public IParserConfiguration RegisterAtlas(Type t);

    /// <summary>
    /// Registers a CLI atlas of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the CLI atlas to register.</typeparam>
    /// <returns>The updated parser configuration.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <typeparamref name="T"/> is null.</exception>
    /// <example>
    /// <code>
    /// var configuration = new ParserConfiguration();
    /// configuration.RegisterAtlas&lt;MyAtlas&gt;();
    /// </code>
    /// </example>
    public IParserConfiguration RegisterAtlas<T>(T atlas) where T : notnull;

    /// <summary>
    /// Registers an atlas from an assembly with the parser configuration.
    /// </summary>
    /// <returns>The updated parser configuration instance.</returns>
    public IParserConfiguration RegisterAtlas(Assembly assembly);

    /// <summary>
    /// Returns the setup information for the parser.
    /// </summary>
    /// <param name="allowOverwrites">Determines whether to allow overwriting of existing configurations.</param>
    /// <returns>The setup information for the parser as a <see cref="ParserDto"/> object.</returns>
    public ParserDto GetParserSetup(bool allowOverwrites = false);

    /// <summary>
    /// Sets the logger for the parser configuration.
    /// </summary>
    /// <param name="logger">The logger to set.</param>
    /// <returns>The updated parser configuration.</returns>
    public IParserConfiguration SetLogger(ILogger logger);

    /// <summary>
    /// Creates an instance of the ArgsParser class.
    /// </summary>
    /// <param name="allowOverwrites">True to allow overwriting of existing configurations, false otherwise.</param>
    /// <returns>An instance of the ArgsParser class.</returns>
    public IParser CreateArgsParser(bool allowOverwrites = false);

    /// <summary>
    /// Creates an instance of the CLI parser.
    /// </summary>
    /// <param name="allowOverwrites">Indicates whether overwriting existing parser configuration is allowed.</param>
    /// <returns>An instance of the CLI parser.</returns>
    public ICliParser CreateCliParser(bool allowOverwrites = false);
}