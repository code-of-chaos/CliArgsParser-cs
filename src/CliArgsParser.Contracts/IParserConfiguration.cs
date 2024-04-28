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

public interface IParserConfiguration {
    public ILogger Log { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IParserConfiguration RegisterAtlas<T>() where T : notnull;
    public IParserConfiguration RegisterAtlas(Type t);
    public IParserConfiguration RegisterAtlas<T>(T atlas) where T : notnull;
    public IParserConfiguration RegisterAtlas(Assembly assembly);

    public ParserDto GetParserSetup(bool allowOverwrites = false);
    
    public IParserConfiguration SetLogger(ILogger logger);

    public IParser CreateArgsParser(bool allowOverwrites = false);
    public IParser CreateCliParser(bool allowOverwrites = false);
}