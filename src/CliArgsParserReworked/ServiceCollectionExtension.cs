// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParserReworked.Contracts;
using CliArgsParserReworked.Parsers;
using CliArgsParserReworked.PreMade;
using Microsoft.Extensions.DependencyInjection;

namespace CliArgsParserReworked;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ServiceCollectionExtension {
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static IServiceCollection AddCliArgsParser(this IServiceCollection services, Action<ICliArgsParserConfiguration> action) {
        CliArgsParserConfiguration configuration = new();
        
        action(configuration);
        
        configuration.AddFromType<ExitAtlas>();
        configuration.AddFromType<HelpAtlas>();
        
        // ReSharper disable once RedundantTypeArgumentsOfMethod
        services.AddSingleton<ICliArgsParser>( provider => new CliArgsParser(provider, configuration.Config) );
        services.AddSingleton<IArgsParser,ArgsParser>();
        services.AddSingleton<ICliParser, CliParser>();
        
        // Add all the types to the service provider, so it should all work together with DI
        foreach (Type configCommandAtlasType in configuration.Config.CommandAtlasTypes)
            // We allow the user to decide which lifetime the atlases will fall under.
            // Default is "Transient"
            services.Add(new ServiceDescriptor(configCommandAtlasType, configCommandAtlasType, configuration.Config.AtlasesServiceLifetime));
        
        foreach (Type configCommandParameterType in configuration.Config.CommandParameterTypes)
            services.AddTransient(configCommandParameterType);

        return services;
    }
}
