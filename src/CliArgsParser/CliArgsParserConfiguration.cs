// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;

namespace CliArgsParser;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc cref="ICliArgsParserConfiguration"/>
public class CliArgsParserConfiguration : ICliArgsParserConfiguration {
    private CliArgsParserConfig? _config;
    internal CliArgsParserConfig Config => _config ?? new CliArgsParserConfig();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc cref="ICliArgsParserConfiguration.SetConfig"/>
    public ICliArgsParserConfiguration SetConfig(CliArgsParserConfig config) {
        foreach (Type type in _config?.CommandAtlasTypes ?? [])
            config.CommandAtlasTypes.Add(type);
        
        foreach (Type type in _config?.CommandParameterTypes ?? [])
            config.CommandParameterTypes.Add(type);
           
        _config = config;
        return this;
    }
    
    /// <inheritdoc cref="ICliArgsParserConfiguration.AddFromAssembly"/>
    public ICliArgsParserConfiguration AddFromAssembly(Assembly assembly) {
        Type[] types = assembly.GetTypes();
        IEnumerable<Type> commandAtlasTypes = types.Where(type => typeof(ICommandAtlas).IsAssignableFrom(type));
        IEnumerable<Type> commandParametersTypes = types.Where(type => typeof(ICommandParameters).IsAssignableFrom(type));

        foreach (Type type in commandAtlasTypes) Config.CommandAtlasTypes.Add(type);
        foreach (Type type in commandParametersTypes) Config.CommandParameterTypes.Add(type);
        
        return this;
    }

    /// <inheritdoc cref="ICliArgsParserConfiguration.AddFromType{T} "/>
    public ICliArgsParserConfiguration AddFromType<T>() where T : ICommandAtlas {
        Type type = typeof(T);
        Config.CommandAtlasTypes.Add(type);
        
        // Extract all the known parameter types
        IEnumerable<Type> parameterTypes =  type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
            .SelectMany(info => info.GetCustomAttributes<CommandAttribute>(inherit: false))
            .Select(attribute => attribute.ArgsType )
        ;
        
        foreach (Type parameterType in parameterTypes) Config.CommandParameterTypes.Add(parameterType);
        
        return this;
    }
}
