// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;
using CliArgsParser.Attributes;
using CliArgsParser.Contracts;
using CliArgsParser.Contracts.Attributes;

namespace CliArgsParser;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a parameter parser for a specific type that implements <see cref="IParameterOptions"/>.
/// </summary>
/// <typeparam name="T">The type of the parameter options.</typeparam>
public class ParameterParser<T> : IParameterParser<T> where T: IParameterOptions, new() {
    // Dictionaries needed to optimize access to the attributes.
    //      Go over and store them once, instead of on every argument like we did before.
    //      Maybe a bit overkill, but might be a good idea in the long run.
    private readonly Dictionary<string, PropertyInfo> _optionProperties = new();

    /// (e.g., "-f", "--flag"), and the values are the corresponding `PropertyInfo` objects representing the flag properties.
    private readonly Dictionary<string, PropertyInfo> _flagProperties = new();

    /// <summary>
    /// Represents an array of PropertyInfo objects for the public instance properties of the generic type T.
    /// </summary>
    private readonly PropertyInfo[] _propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ParameterParser() {
        foreach (PropertyInfo? prop in _propertyInfos) {
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

    /// <summary>
    /// Returns a collection of readable descriptions for the arguments defined in the parameter options.
    /// Each description includes the short name, long name, and description of the argument.
    /// If the description is not provided, "UNKNOWN DESCRIPTION" is used.
    /// </summary>
    /// <returns>A collection of readable descriptions for the arguments.</returns>
    public IEnumerable<string> GetDescriptionsReadable() {
        return GetDescriptions<ArgAttribute>()
            .Select(v => $"-{v?.ShortName,-3} --{v?.LongName,-8} : {v?.Description ?? "UNKNOWN DESCRIPTION"}");
    }

    /// <summary>
    /// Retrieves the descriptions of the command line arguments in a readable format.
    /// </summary>
    /// <typeparam name="TT">The attribute type to filter the descriptions.</typeparam>
    /// <returns>
    /// An <see cref="IEnumerable{TT}"/> containing the descriptions of the command line arguments
    /// in a readable format.
    /// </returns>
    public IEnumerable<TT?> GetDescriptions<TT>() where TT : Attribute, IArgAttribute {
        return _propertyInfos
            .Select(value => value.GetCustomAttribute<TT>());
    }

    /// <summary>
    /// Parses the command-line arguments and returns an instance of the specified parameter options type.
    /// </summary>
    /// <typeparam name="T">The type of the parameter options.</typeparam>
    /// <param name="args">The command-line arguments.</param>
    /// <returns>An instance of the specified parameter options type.</returns>
    public T Parse(IEnumerable<string> args) {
        var result = new T();
        string[] enumerable = args as string[] ?? args.ToArray();

        for (var i = 0; i < enumerable.Length; i++) {
            // Eh, this isn't great
            //      Currently if one parameter is verbose, the entire thing will be flagged as verbose
            //      TODO maybe in some way add this to the properties to see which one is verbose or not?
            bool isVerbose = enumerable[i].StartsWith("--");
            string argName = enumerable[i].ToLower();
            
            if (_optionProperties.TryGetValue(argName, out PropertyInfo? optionProp) && i < enumerable.Length - 1) {
                object value = Convert.ChangeType(enumerable[++i], optionProp.PropertyType); // cast to the correct type of the param
                optionProp.SetValue(result, value);
            }
            else if (_flagProperties.TryGetValue(argName, out PropertyInfo? flagProp)) {
                flagProp.SetValue(result, true);
            }
            
            // Apply the verbose in correct location, ergo: HERE
            result.Verbose = result.Verbose || isVerbose;
        }

        return result;
    }
}