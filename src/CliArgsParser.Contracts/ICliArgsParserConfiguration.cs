// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Contracts.Types;
using System.Reflection;

namespace CliArgsParser.Contracts;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICliArgsParserConfiguration {
    ICliArgsParserConfiguration SetConfig(CliArgsParserConfig config);
    
    ICliArgsParserConfiguration AddFromAssembly(Assembly assembly);
    ICliArgsParserConfiguration AddFromType<T>() where T : ICommandAtlas;
}
