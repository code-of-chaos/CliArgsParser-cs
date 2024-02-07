﻿// ---------------------------------------------------------------------------------------------------------------------
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
public class ParameterParser<T> : IParameterParser<T> where T: IParameterOptions, new() {
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
    
    public IEnumerable<string> GetDescriptionsReadable() {
        return GetDescriptions<ArgAttribute>()
            .Select(v => $"-{v?.ShortName,-3} --{v?.LongName,-8} : {v?.Description ?? "UNKNOWN DESCRIPTION"}");
    }

    public IEnumerable<TT?> GetDescriptions<TT>() where TT : Attribute, IArgAttribute {
        return _propertyInfos
            .Select(value => value.GetCustomAttribute<TT>());
    }
    
    public T Parse(IEnumerable<string> args) {
        var result = new T();
        string[]? enumerable = args as string[] ?? args.ToArray();

        for (var i = 0; i < enumerable.Length; i++) {
            // Eh, this isn't great
            //      Currently if one parameter is verbose, the entire thing will be flagged as verbose
            //      TODO maybe in some way add this to the properties to see which one is verbose or not?
            bool isVerbose = enumerable[i].StartsWith("--");
            string argName = enumerable[i].ToLower();
            
            if (_optionProperties.TryGetValue(argName, out PropertyInfo? optionProp) && i < enumerable.Length - 1) {
                object? value = Convert.ChangeType(enumerable[++i], optionProp.PropertyType);
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