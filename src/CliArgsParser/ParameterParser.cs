
// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;
using CliArgsParser.Contracts;
using CliArgsParser.Contracts.Attributes;

namespace CliArgsParser;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public class ParameterParser : IParameterParser {
    private readonly Dictionary<string, PropertyInfo> _valueProperties = new();
    private readonly Dictionary<string, PropertyInfo> _flagProperties = new();

    /// <inheritdoc />
    public Type ParamsType { get; private set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Represents a parameter parser that is responsible for parsing command-line arguments into parameters.
    /// </summary>
    public ParameterParser(Type type) {
        ParamsType = type;
        PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance );
        
        foreach (PropertyInfo? prop in propertyInfos) {
            var valueAttr = prop.GetCustomAttribute<ArgValue>();
            var flagAttr = prop.GetCustomAttribute<ArgFlag>();

            if (valueAttr != null) {
                _valueProperties[valueAttr.ShortName] = prop;
                _valueProperties[valueAttr.LongName] = prop;
            } else if (flagAttr != null) {
                _flagProperties[flagAttr.ShortName] = prop;
                _flagProperties[flagAttr.LongName] = prop;
            }
        }
    }

    /// <inheritdoc />
    public IParameters? Parse(Dictionary<string, string> args) {
        var result = (IParameters?)Activator.CreateInstance(ParamsType);
        if (result == null) {
            return null;
        }
    
        foreach ((string key, string value) in args) {
            if (_valueProperties.TryGetValue(key, out PropertyInfo? optionProp)) {
                object v = Convert.ChangeType(value, optionProp.PropertyType); // cast to the correct type of the param
                optionProp.SetValue(result, v);
            }
            else if (_flagProperties.TryGetValue(key, out PropertyInfo? flagProp)) {
                flagProp.SetValue(result, true);
            }
        }
        
        return result;
    }
}