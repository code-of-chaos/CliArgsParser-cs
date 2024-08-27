// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace CliArgsParser;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc cref="ICommandParameterParser"/>
public class CommandParameterParser(Type type, IServiceProvider provider) : ICommandParameterParser {
    private readonly ImmutableDictionary<string, PropertyInfo> _valueProperties = AssembleDict<ArgValueAttribute>(type);
    private readonly ImmutableDictionary<string, PropertyInfo> _flagProperties = AssembleDict<ArgFlagAttribute>(type);

    /// <inheritdoc cref="ICommandParameterParser.ParamsType"/>
    public Type ParamsType { get; } = type;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Assembles an immutable dictionary of properties annotated with the specified attribute type.
    /// The dictionary keys are the names or short names of the attributes.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <param name="type">The type to search for annotated properties.</param>
    /// <returns>An immutable dictionary of properties annotated with the specified attribute type.</returns>
    private static ImmutableDictionary<string, PropertyInfo> AssembleDict<TAttribute>(Type type) where TAttribute : Attribute, IAttributeWithName {
        return new Dictionary<string, PropertyInfo>(
            type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .SelectMany(info => info.GetCustomAttributes<TAttribute>().Select(attribute => (info, Attribute: attribute)))
                .SelectMany<(PropertyInfo info, TAttribute Attribute), KeyValuePair<string, PropertyInfo>>(tuple => [
                    new KeyValuePair<string, PropertyInfo>(tuple.Attribute.Name, tuple.info),
                    new KeyValuePair<string, PropertyInfo>(tuple.Attribute.ShortName ?? tuple.Attribute!.Name[0].ToString(), tuple.info)
                ])
        ).ToImmutableDictionary();
    }

    /// <inheritdoc cref="ICommandParameterParser.TryParse"/>
    public bool TryParse(Dictionary<string, string> args, [NotNullWhen(true)] out ICommandParameters? parameters) {
        parameters = null;
        if (provider.GetService(ParamsType) is not ICommandParameters result) return false;

        foreach ((string key, string value) in args) {
            if (_valueProperties.TryGetValue(key, out PropertyInfo? optionProp)) {
                object v = Convert.ChangeType(value, optionProp.PropertyType);// cast to the correct type of the param
                optionProp.SetValue(result, v);
            }
            else if (_flagProperties.TryGetValue(key, out PropertyInfo? flagProp)) {
                // Tries and parses the value, defaults to "true" value
                if (string.IsNullOrEmpty(value)) flagProp.SetValue(result, true);// Default is that no flag value is set, so is presumed 
                else if (bool.TryParse(value, out bool v)) flagProp.SetValue(result, v);
                else flagProp.SetValue(result, true);
            }
        }

        parameters = result;
        return true;
    }
}
