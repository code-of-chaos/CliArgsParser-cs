// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Contracts;
using CliArgsParser.Contracts.Attributes;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace CliArgsParser;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ParameterParser(Type type, IServiceProvider provider) : IParameterParser {
    private readonly ImmutableDictionary<string, PropertyInfo> _valueProperties = AssembleDict<ArgValueAttribute>(type);
    private readonly ImmutableDictionary<string, PropertyInfo> _flagProperties = AssembleDict<ArgFlagAttribute>(type);
    public Type ParamsType { get; } = type;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static ImmutableDictionary<string, PropertyInfo> AssembleDict<TAttribute>(Type type) where TAttribute : Attribute, IAttributeWithName {
        return new Dictionary<string, PropertyInfo>(
            type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .SelectMany(info => info.GetCustomAttributes<TAttribute>().Select(attribute => (info, Attribute:attribute )))
                .SelectMany<(PropertyInfo info, TAttribute Attribute), KeyValuePair<string, PropertyInfo>>(tuple =>  [
                    new KeyValuePair<string, PropertyInfo>(tuple.Attribute.Name, tuple.info),
                    new KeyValuePair<string, PropertyInfo>(tuple.Attribute.ShortName ?? tuple.Attribute!.Name[0].ToString(), tuple.info)
                ])
        ).ToImmutableDictionary();
    }
    
    public bool TryParse(Dictionary<string, string> args, [NotNullWhen(true)] out IParameters? parameters) {
        parameters = null;
        if (provider.GetService(ParamsType) is not IParameters result) return false;
        
        foreach ((string key, string value) in args) {
            if (_valueProperties.TryGetValue(key, out PropertyInfo? optionProp)) {
                object v = Convert.ChangeType(value, optionProp.PropertyType);// cast to the correct type of the param
                optionProp.SetValue(result, v);
            }
            else if (_flagProperties.TryGetValue(key, out PropertyInfo? flagProp)) {
                // Tries and parses the value, defaults to "true" value
                if (string.IsNullOrEmpty(value)) flagProp.SetValue(result, true); // Default is that no flag value is set, so is presumed 
                else if (bool.TryParse(value, out bool v)) flagProp.SetValue(result, v);
                else flagProp.SetValue(result, true);
            }
        }

        parameters = result;
        return true;
    }
}
