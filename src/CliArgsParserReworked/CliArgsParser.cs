// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParserReworked.Contracts;
using CliArgsParserReworked.Contracts.Attributes;
using CliArgsParserReworked.Contracts.Data;
using CliArgsParserReworked.Contracts.Types;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CliArgsParserReworked;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CliArgsParser(IServiceProvider provider, CliArgsParserConfig configuration) : ICliArgsParser {
    public CliArgsParserConfig Config { get; } = configuration;
    
    private ImmutableDictionary<Type, IParameterParser>? _parameterParsers;
    public ImmutableDictionary<Type, IParameterParser> parameterParsers => _parameterParsers ??= GetParameterParsersMap(provider, Config);

    private ImmutableDictionary<string, CommandMethodInfo>? _commands;
    public ImmutableDictionary<string, CommandMethodInfo> Commands => _commands ??= GetCommandsMap(provider, Config);
    // -----------------------------------------------------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------------------------------------------------
    
    #region Constructor Logic
    private static ImmutableDictionary<Type, IParameterParser> GetParameterParsersMap(IServiceProvider provider, CliArgsParserConfig configuration) {
        return  new Dictionary<Type,IParameterParser>(
            configuration.CommandParameterTypes.Select(
                type => new KeyValuePair<Type, IParameterParser>(type, new ParameterParser(type,provider))
            )
        ).ToImmutableDictionary();
    }

    public static ImmutableDictionary<string, CommandMethodInfo> GetCommandsMap(IServiceProvider provider, CliArgsParserConfig configuration) {
        Dictionary<string, int> duplicateNameCache = new();
        Dictionary<string, CommandMethodInfo> commands = new();

        foreach (Type commandAtlasType in configuration.CommandAtlasTypes) {
            IEnumerable<(string, CommandMethodInfo)> enumerable = commandAtlasType
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                .Select(info => (info, Attribute: info.GetCustomAttribute<CommandAttribute>(), DescriptionAttribute: info.GetCustomAttribute<DescriptionAttribute>()))
                .Where(tuple => tuple.Attribute is not null)
                .Select(tuple => new CommandMethodInfo(
                    tuple.info,
                    tuple.Attribute!,// can be suppressed because we already filter in "where"
                    (ICommandAtlas)ActivatorUtilities.CreateInstance(provider, commandAtlasType),
                    tuple.DescriptionAttribute
                ))
                .SelectMany(cmdInfo => {
                    var list = new List<(string, CommandMethodInfo)>();

                    string? tempName = null;
                    if (cmdInfo.CommandAttribute.Name is {} name) {
                        list.Add((name, cmdInfo));
                        tempName = name;
                    }
                    else if (RegexLib.SplitCamelCase.Matches(cmdInfo.Info.Name) is {} matches) {
                        List<Capture> groups = matches.SelectMany(match => match.Captures).ToList();
                        IEnumerable<Capture> sections = string.Equals(groups[0].Value, "command", StringComparison.InvariantCultureIgnoreCase)
                            ? groups.Skip(1)
                            : groups;
                        tempName = string.Join("-", sections.Select(s => s.Value.ToLowerInvariant()).Where(s => !string.IsNullOrEmpty(s) || !string.IsNullOrWhiteSpace(s)));
                        list.Add((tempName, cmdInfo));
                    }

                    if (!configuration.GenerateShortNames || tempName is null) return list;

                    name = string.Join(string.Empty, tempName.Split("-").Select(s => s.Trim()[0]));
                    list.Add((name, cmdInfo));

                    return list;
                });

            // Assemble the command dictionary and append numbers to duplicates
            foreach ((string name, CommandMethodInfo info) in enumerable) {
                string suffix = string.Empty;
                int count;
                if (duplicateNameCache.TryGetValue(name, out count)) {
                    suffix = count.ToString();
                    duplicateNameCache[name] = count + 1;
                }
                else {
                    duplicateNameCache[name] = 1;
                }

                if (commands.TryAdd($"{name}{suffix}", info)) continue;
                Console.WriteLine($"name duplicate {name}");
            }
        }

        return commands.ToImmutableDictionary();
    }
    #endregion
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    
    private static bool TryGetCommand(string input, [NotNullWhen(true)] out string? commandName, out Dictionary<string, string> args) {
        commandName = null;
        args = new Dictionary<string, string>();

        string[] strings = input.Split(" ", 2);

        if (strings.Length < 1) return false;

        commandName = strings[0];
        if (strings.Length == 2) args = GetArgs(strings[1]);

        return true;
    }
    
    private static Dictionary<string, string> GetArgs(string argsInput) {
        return RegexLib.Args
            .Matches(argsInput)
            .Where(match => match.Success)
            .ToDictionary(
                keySelector: match => match.Groups[1].Value,
                elementSelector: match => match.Groups[2].Success
                    ? match.Groups[2].Value.Trim('"')
                    : "True"
            );
    }
    
    public async Task ExecuteAsync(string commandString) {
        if (!TryGetCommand(commandString, out string? commandName, out Dictionary<string, string>? args))
            throw new ArgumentException("Invalid command structure");

        if (!Commands.TryGetValue(commandName, out CommandMethodInfo? commandMethodInfo)) 
            throw new ArgumentException("Invalid command name");

        if (!parameterParsers.TryGetValue(commandMethodInfo.ParameterType, out IParameterParser? parser)
            || !parser.TryParse(args, out IParameters? parameters)
        ) throw new ArgumentException("Invalid command parameter type");


        switch (commandMethodInfo.Delegate) {
            case Func<Task> func when parameters is null or NoArgs:
                await func();// For Async methods without parameters
                return;

            case Action action when parameters is null or NoArgs:
                action();// For non-async methods without parameters
                return;

            case {} del when commandMethodInfo.IsAsync:
                if (parameters == null) {
                    throw new ArgumentException("No parameters provided for async method that needs parameters");
                }

                var task = (Task)del.DynamicInvoke(parameters)!;
                await task;
                return;

            case {} del:
                if (parameters == null) {
                    throw new ArgumentException("No parameters provided for method that needs parameters");
                }

                del.DynamicInvoke(parameters);// For non-async methods with parameters
                return;

            default:
                throw new ConstraintException("Delegate could not be invoked");
        }
    }
}
