// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParserReworked.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace CliArgsParserReworked;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ServiceCollectionExtension {
    public static IServiceCollection AddCliArgsParser(this IServiceCollection services, Action<ICliArgsParserConfiguration> action) {
        CliArgsParserConfiguration configuration = new();
        
        action(configuration);
        
        services.AddSingleton<CliArgsParserFactory>();
        
        // ReSharper disable once RedundantTypeArgumentsOfMethod
        services.AddSingleton<CliArgsParser>(ImplementationFactory);
        services.AddSingleton<ICliArgsParser>(ImplementationFactory);
        
        // Add all the types to the service provider, so it should all work together with DI
        foreach (Type configCommandAtlasType in configuration.Config.CommandAtlasTypes)
            // We allow the user to decide which lifetime the atlases will fall under.
            // Default is "Transient"
            services.Add(new ServiceDescriptor(configCommandAtlasType, configCommandAtlasType, configuration.Config.AtlasesServiceLifetimes));
        
        foreach (Type configCommandParameterType in configuration.Config.CommandParameterTypes)
            services.AddTransient(configCommandParameterType);

        return services;
        
        CliArgsParser ImplementationFactory(IServiceProvider provider) =>
            CliArgsParserFactory.BuildCliArgsParser(provider, configuration.Config);
    }
}
