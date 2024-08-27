// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

namespace CliArgsParser;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Extension to an <see cref="IServiceCollection"/> to easily add the required classes and interfaces to the service collection.
/// </summary>
public static class ServiceCollectionExtension {
    #region Helper Methods
    private static CliArgsParserConfiguration ProcessActions(Action<ICliArgsParserConfiguration> action) {
        CliArgsParserConfiguration configuration = new();

        action(configuration);

        if (configuration.Config.EnableHelpAtlas) configuration.AddFromType<HelpAtlas>();
        if (configuration.Config.EnableExitAtlas) configuration.AddFromType<ExitAtlas>();

        return configuration;
    }
    private static IServiceCollection ProcessServices(IServiceCollection services, CliArgsParserConfiguration configuration) {
        services.AddSingleton<ICliArgsParser>(provider => new CliArgsParser(provider, configuration.Config));

        // Add all the types to the service provider, so it should all work together with DI
        foreach (Type configCommandAtlasType in configuration.Config.CommandAtlasTypes)
            // We allow the user to decide which lifetime the atlases will fall under.
            // Default is "Transient"
        {
            services.Add(new ServiceDescriptor(configCommandAtlasType, configCommandAtlasType, configuration.Config.AtlasesServiceLifetime));
        }

        foreach (Type configCommandParameterType in configuration.Config.CommandParameterTypes) {
            services.AddTransient(configCommandParameterType);
        }

        return services;
    }
    #endregion

    /// <summary>
    /// Adds both <see cref="ArgsParser"/> and <see cref="CliParser"/> to the IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the CliArgsParser to.</param>
    /// <param name="action">The configuration action to apply to the CliArgsParser.</param>
    /// <returns>The modified IServiceCollection.</returns>
    public static IServiceCollection AddCliArgsParser(this IServiceCollection services, Action<ICliArgsParserConfiguration> action) {
        CliArgsParserConfiguration configuration = ProcessActions(action);

        services.AddSingleton<ICliParser, CliParser>();
        services.AddSingleton<IArgsParser, ArgsParser>();

        return ProcessServices(services, configuration);
    }

    /// <summary>
    /// Adds the <see cref="ArgsParser"/> to the IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the CliArgsParser to.</param>
    /// <param name="action">The configuration action to apply to the CliArgsParser.</param>
    /// <returns>The modified IServiceCollection.</returns>
    public static IServiceCollection AddArgsParser(this IServiceCollection services, Action<ICliArgsParserConfiguration> action) {
        CliArgsParserConfiguration configuration = ProcessActions(action);

        services.AddSingleton<IArgsParser, ArgsParser>();

        return ProcessServices(services, configuration);
    }

    /// <summary>
    /// Adds the <see cref="CliParser"/> to the IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the CliArgsParser to.</param>
    /// <param name="action">The configuration action to apply to the CliArgsParser.</param>
    /// <returns>The modified IServiceCollection.</returns>
    public static IServiceCollection AddCliParser(this IServiceCollection services, Action<ICliArgsParserConfiguration> action) {
        CliArgsParserConfiguration configuration = ProcessActions(action);

        services.AddSingleton<ICliParser, CliParser>();

        return ProcessServices(services, configuration);
    }
}
