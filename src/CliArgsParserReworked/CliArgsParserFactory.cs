// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParserReworked.Contracts;
using CliArgsParserReworked.Contracts.Attributes;
using CliArgsParserReworked.Contracts.Data;
using CliArgsParserReworked.Contracts.Types;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CliArgsParserReworked;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CliArgsParserFactory {
    private static ImmutableDictionary<Type, ParameterParser> GetParameterParsersMap(IServiceProvider provider, CliArgsParserConfig configuration) {
        return  new Dictionary<Type,ParameterParser>(
            configuration.CommandParameterTypes.Select(
                type => new KeyValuePair<Type, ParameterParser>(type, new ParameterParser(type,provider))
            )
        ).ToImmutableDictionary();
    }
    
    public static CliArgsParser BuildCliArgsParser(IServiceProvider provider, CliArgsParserConfig configuration) {
        Dictionary<string, int> shortNameCache = new();
        Dictionary<string, CommandMethodInfo> commands = new();
        foreach (Type commandAtlasType in configuration.CommandAtlasTypes) {
            IEnumerable<(string, CommandMethodInfo)> enumerable = commandAtlasType
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                .Select(info => (info, Attribute: info.GetCustomAttribute<CommandAttribute>()))
                .Where(tuple => tuple.Attribute is not null)
                .Select(tuple => new CommandMethodInfo(
                    tuple.info,
                    tuple.Attribute!,// can be suppressed because we already filter in "where"
                    (ICommandAtlas)ActivatorUtilities.CreateInstance(provider, commandAtlasType)
                ))
                .SelectMany(cmdInfo => {
                    var list = new List<(string, CommandMethodInfo)>();

                    string? tempName = null;
                    if (cmdInfo.CommandAttribute.Name is {} name) {
                        list.Add((name, cmdInfo));
                        tempName = name;
                    }
                    
                    if (RegexLib.SplitCamelCase.Matches(cmdInfo.Info.Name) is {} matches) {
                        List<Capture> groups = matches.SelectMany(match => match.Captures).ToList();
                        IEnumerable<Capture> sections = string.Equals(groups[0].Value, "command", StringComparison.InvariantCultureIgnoreCase)
                            ? groups.Skip(1)
                            : groups;
                        tempName = string.Join("-", sections.Select(s => s.Value.ToLowerInvariant()));
                        list.Add((tempName, cmdInfo));
                    }

                    if (configuration.GenerateShortNames && tempName is not null) {
                        name = tempName.Split("-").Aggregate(string.Empty, (current, next) => current + $"-{next[0]}");

                        if (shortNameCache.TryGetValue(name, out int i) && i > 0) {
                            name = $"{name}{i - 1}";
                        }

                        list.Add((name, cmdInfo));
                        shortNameCache.Add(name, i);
                    }

                    return list;
                });

            foreach ((string, CommandMethodInfo) tuple in enumerable) {
                commands.Add(tuple.Item1, tuple.Item2);
            }
        }

        return new CliArgsParser(
            GetParameterParsersMap(provider, configuration), 
            configuration,
            commands
        );
    }
}
