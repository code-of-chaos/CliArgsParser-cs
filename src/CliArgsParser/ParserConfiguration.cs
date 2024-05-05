// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using CliArgsParser.Attributes;
using CliArgsParser.Contracts;
using CliArgsParser.Contracts.Common;
using CliArgsParser.PreMade.Parsers;
using Serilog;
using Serilog.Core;

namespace CliArgsParser;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

/// <inheritdoc />
public class ParserConfiguration : IParserConfiguration {
    private readonly LinkedList<object> _linkedAtlases = [];
    
    /// <inheritdoc />
    public ILogger Log { get; private set; } = Logger.None;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods - Registering
    // -----------------------------------------------------------------------------------------------------------------
    
    /// <inheritdoc />
    public IParserConfiguration RegisterAtlas<T>() where T : notnull => RegisterAtlas(typeof(T));
    
    /// <inheritdoc />
    public IParserConfiguration RegisterAtlas(Type t) {
        ConstructorInfo? loggerConstructor = t.GetConstructor([typeof(ILogger)]);
        ConstructorInfo? parameterlessConstructor = t.GetConstructor(Type.EmptyTypes);

        if (loggerConstructor != null) {
            Log.Information("CliArgsParser : Creating an instance of type {t} with a ILogger parameter", t);
            RegisterAtlas(Activator.CreateInstance(t, Log)!);
        }
        else if (parameterlessConstructor != null) {
            Log.Information("CliArgsParser : Creating an instance of type {t} without any parameters", t);
            RegisterAtlas(Activator.CreateInstance(t)!);
        }
        else {
            Log.Error("CliArgsParser : Unable to create instance of type {t}. Type must have parameterless or a single parameter constructor of type ILogger.", t);
            throw new InvalidOperationException($"Unable to create instance of type {t}. Type must have parameterless or a single parameter constructor of type ILogger.");
        }

        return this;
    } 
    
    /// <inheritdoc />
    public IParserConfiguration RegisterAtlas(Assembly assembly) {
        assembly.ExportedTypes
            .Where(t => 
                Attribute.IsDefined(t, typeof(CommandAtlasAttribute)) 
            )
            .ToList()
            .ForEach(t => RegisterAtlas(t));
   
        return this;
    }
    
    /// <inheritdoc />
    public IParserConfiguration RegisterAtlas<T>(T atlas) where T : notnull {
        CommandAtlasAttribute? attribute = atlas.GetType()
            .GetCustomAttributes(typeof(CommandAtlasAttribute), false)
            .OfType<CommandAtlasAttribute>()
            .FirstOrDefault();
        
        if (attribute == null) {
            Type attributeType = typeof(CommandAtlasAttribute);
            Log.Error("CliArgsParser : The object {atlas} did not have an attribute {attributeName}", atlas,attributeType);
            throw new Exception($"The object {atlas} did not have an attribute {attributeType}");
        }

        _linkedAtlases.AddLast(atlas);
        Log.Information("CliArgsParser : Registered CLI atlas {atlas}", atlas.GetType().Name);

        return this;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods - extra
    // -----------------------------------------------------------------------------------------------------------------
    
    /// <inheritdoc />
    public IParserConfiguration SetLogger(ILogger logger) {
        Log = logger;
        Log.Information("CliArgsParser : Set new Logger");
        
        return this;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods - Create Parser
    // -----------------------------------------------------------------------------------------------------------------
    private Dictionary<string, CommandRecord> AssembleDictionary(bool allowOverwrites = false) {
        var commandDictionary = new Dictionary<string, CommandRecord>();

        _linkedAtlases
            .Select(atlas => new {Object = atlas, Type =  atlas.GetType()})
            .SelectMany(o => o.Type
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                .Where(m => m.DeclaringType != typeof(object))
                .Select(info => new CommandMethodInfo(info, o.Object, Log))
                .Where(cm => cm.CommandAttribute != null)
                .Select(cm => new CommandRecord(
                    Name: cm.CommandAttribute!.Name,
                    Description: cm.CommandAttribute!.Description,
                    Delegate: cm.Delegate,
                    ReturnType: cm.Info.ReturnType,
                    IsAsync: cm.IsAsync,
                    ParameterParser:cm.ParameterParser
                )
            ))
            .ToList()
            .ForEach(record => {
                if (!commandDictionary.TryAdd(record.Name, record) && !allowOverwrites) {
                    Log.Error("CliArgsParser : Command with name '{name}' already exists and overwriting is not allowed.", record.Name);
                } else {
                    Log.Information("CliArgsParser : Registered command {name} to atlas", record.Name);
                }
            });

        return commandDictionary;
    }

    /// <inheritdoc />
    public ParserDto GetParserSetup(bool allowOverwrites = false) {
        Dictionary<string, CommandRecord> dictionary = AssembleDictionary(allowOverwrites);
        
        return new ParserDto(
            Log,
            dictionary.Any(pair => pair.Value.IsAsync),
            dictionary
        );
    } 
    
    /// <inheritdoc />
    public IParser CreateArgsParser(bool allowOverwrites = false) => new ArgsParser().IngestFromSetup(GetParserSetup());
    
    /// <inheritdoc />
    public IParser CreateCliParser(bool allowOverwrites = false) => new CliParser().IngestFromSetup(GetParserSetup());
}