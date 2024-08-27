// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Parsers;
using CliArgsParser.PreMade;
using CliArgsParser.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace CliArgsParser;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ServiceCollectionExtension {
    #region Helper Methods
    private static CliArgsParserConfiguration ProcessActions(Action<ICliArgsParserConfiguration> action) {
        CliArgsParserConfiguration configuration = new();
        
        action(configuration);
        
        if(configuration.Config.EnableHelpAtlas) configuration.AddFromType<HelpAtlas>();
        if(configuration.Config.EnableExitAtlas) configuration.AddFromType<ExitAtlas>();
        
        return configuration;
    }
    private static IServiceCollection ProcessServices(IServiceCollection services, CliArgsParserConfiguration configuration) {
        services.AddSingleton<ICliArgsParser>( provider => new CliArgsParser(provider, configuration.Config) );
        
        // Add all the types to the service provider, so it should all work together with DI
        foreach (Type configCommandAtlasType in configuration.Config.CommandAtlasTypes)
            // We allow the user to decide which lifetime the atlases will fall under.
            // Default is "Transient"
            services.Add(new ServiceDescriptor(configCommandAtlasType, configCommandAtlasType, configuration.Config.AtlasesServiceLifetime));
        
        foreach (Type configCommandParameterType in configuration.Config.CommandParameterTypes)
            services.AddTransient(configCommandParameterType);

        return services;
    }
    #endregion
    
    public static IServiceCollection AddCliArgsParser(this IServiceCollection services, Action<ICliArgsParserConfiguration> action) {
        CliArgsParserConfiguration configuration = ProcessActions(action);
        
        services.AddSingleton<ICliParser, CliParser>();
        services.AddSingleton<IArgsParser,ArgsParser>();
        
        return ProcessServices(services, configuration);
    }
    
    public static IServiceCollection AddArgsParser(this IServiceCollection services, Action<ICliArgsParserConfiguration> action) {
        CliArgsParserConfiguration configuration = ProcessActions(action);
        
        services.AddSingleton<IArgsParser,ArgsParser>();
        
        return ProcessServices(services, configuration);
    }
    
    public static IServiceCollection AddCliParser(this IServiceCollection services, Action<ICliArgsParserConfiguration> action) {
        CliArgsParserConfiguration configuration = ProcessActions(action);
        
        services.AddSingleton<ICliParser, CliParser>();
        
        return ProcessServices(services, configuration);
    }
}
