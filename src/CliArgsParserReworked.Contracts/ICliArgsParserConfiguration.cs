// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParserReworked.Contracts.Types;
using System.Reflection;

namespace CliArgsParserReworked.Contracts;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICliArgsParserConfiguration {
    ICliArgsParserConfiguration SetConfig(CliArgsParserConfig config);
    
    ICliArgsParserConfiguration AddFromAssembly(Assembly assembly);
    ICliArgsParserConfiguration AddFromType<T>() where T : ICommandAtlas;
}
