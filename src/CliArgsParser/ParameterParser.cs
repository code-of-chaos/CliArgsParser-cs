// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;
using CliArgsParser.Attributes;
using CliArgsParser.Contracts;

namespace CliArgsParser;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ParameterParser<T> : IParamaterParser where T: IParameterOptions, new() {
    // Dictionaries needed to optimize access to the attributes.
    //      Go over and store them once, instead of on every argument like we did before.
    //      Maybe a bit overkill, but might be a good idea in the long run.
    private readonly Dictionary<string, PropertyInfo> _optionProperties = new();
    private readonly Dictionary<string, PropertyInfo> _flagProperties = new();
    private readonly PropertyInfo[] _propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ParameterParser() {
        foreach (var prop in _propertyInfos) {
            var optionAttr = prop.GetCustomAttribute<ArgValueAttribute>();
            var flagAttr = prop.GetCustomAttribute<ArgFlagAttribute>();

            if (optionAttr != null) {
                _optionProperties[$"-{optionAttr.ShortName}"] = prop;
                _optionProperties[$"--{optionAttr.LongName}"] = prop;
            } else if (flagAttr != null) {
                _flagProperties[$"-{flagAttr.ShortName}"] = prop;
                _flagProperties[$"--{flagAttr.LongName}"] = prop;
            }
        }
    }
    
    public IEnumerable<string> GetDescriptionsReadable() {
        return GetDescriptions()
            .Select(v => $"-{v?.ShortName,-3} --{v?.LongName,-8} : {v?.Description ?? "UNKNOWN DESCRIPTION"}");
    }
    
    public IEnumerable<ArgAttribute?> GetDescriptions() {
        return _propertyInfos
            .Select(value => value.GetCustomAttribute<ArgAttribute>());
    }
    
    public T Parse(string[] args) {
        var result = new T();

        for (int i = 0; i < args.Length; i++) {
            // Eh, this isn't great
            //      Currently if one parameter is verbose, the entire thing will be flagged as verbose
            //      TODO maybe in some way add this to the properties to see which one is verbose or not?
            bool isVerbose = args[i].StartsWith("--");
            string argName = args[i].ToLower();
            
            if (_optionProperties.TryGetValue(argName, out var optionProp) && i < args.Length - 1) {
                var value = Convert.ChangeType(args[++i], optionProp.PropertyType);
                optionProp.SetValue(result, value);
            }
            else if (_flagProperties.TryGetValue(argName, out var flagProp)) {
                flagProp.SetValue(result, true);
            }
            
            // Apply the verbose in correct location, ergo: HERE
            result.Verbose = result.Verbose || isVerbose;
        }

        return result;
    }
}