// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;
using CliArgsParser.Commands;
using CliArgsParser.Contracts;
using CliArgsParser.Contracts.Attributes;
using CliArgsParser.Contracts.Delegates;

namespace CliArgsParser;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CliArgsParser : ICliArgsParser {
    private readonly Dictionary<string, CommandStruct> _flagToActionMap = new();
    
    private static readonly Dictionary<string, string?> _descriptions = new();
    public static IReadOnlyDictionary<string, string?> Descriptions => _descriptions.AsReadOnly(); // Again added for the future, don't know what to add to it.

    public static string Cursor = "> ";
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructor
    // -----------------------------------------------------------------------------------------------------------------
    public CliArgsParser(string cursor = "> ", bool addDefault = true) {
        // There is two reserved commands "HELP", which lists all command, and "EXIT", which exists when in input mode
        //      Though I have now added this as an optional, this should still work fine ...
        if (addDefault) {
            RegisterFromCliAtlas(new DefaultCommands());
        }

        Cursor = cursor;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ICliArgsParser RegisterFromCliAtlas<T>(IEnumerable<T> cliCommandAtlas, bool force = false) where T : ICliCommandAtlas {
        foreach (var atlas in cliCommandAtlas) {
            RegisterFromCliAtlas(atlas);
        }
        return this;
    }
    
    public ICliArgsParser RegisterFromCliAtlas<T>(T cliCommandAtlas, bool force = false) where T:ICliCommandAtlas{
        var methods = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Instance);

        foreach (var methodInfo in methods) {
            // Quick exits to find the correct methods
            if (methodInfo.GetCustomAttributes().FirstOrDefault(a => a is ICliCommandAttribute) is not ICliCommandAttribute cliCommandAttribute) continue;
            var methodParameters = methodInfo.GetParameters();
            if (methodParameters.Length == 0) continue;
            
            string commandName = cliCommandAttribute.Name.ToLower();
            
            Delegate del;
            
            try {
                Type delegateType = typeof(CommandCallback<>).MakeGenericType(cliCommandAttribute.ParameterOptionsType);
                del = Delegate.CreateDelegate(delegateType, cliCommandAtlas, methodInfo);
            } catch {
                Console.WriteLine($"Attempting to bind method {methodInfo.Name} with return type {methodInfo.ReturnType.Name} and parameters {String.Join(",", methodParameters.Select(p => p.ParameterType.FullName))}");
                throw;
            }

            if (_flagToActionMap.TryAdd(commandName, new CommandStruct(del, cliCommandAttribute))) {
                _descriptions.Add(commandName, cliCommandAttribute.Description);
                continue;
            }
            
            // if something fails
            //      Don't write the same string twice
            string errorText = $"command '{commandName}' could not be bound to method '{typeof(T)}.{methodInfo.Name}'"; 
            if (!force) {
                Console.WriteLine($"Ignoring: {errorText}");
            } else {
                throw new Exception(errorText);
            }
        }

        // added for easy chaining
        return this;
    }
    
    // TODO test this out
    public ICliArgsParser RegisterFromDlLs(IEnumerable<string> filePaths) {
        foreach (var filePath in filePaths) {
            Assembly assembly = Assembly.LoadFrom(filePath);

            foreach (var objectType in assembly.GetTypes()) {
                if (!typeof(ICliCommandAtlas).IsAssignableFrom(objectType)
                    || objectType is not { IsInterface: false, IsAbstract: false }) continue;

                // Actually register the commands
                ICliCommandAtlas? cliCommandAtlas = (ICliCommandAtlas?)Activator.CreateInstance(objectType);

                if (cliCommandAtlas is null) {
                    // Something went wrong
                    throw new Exception($"Command atlas '{objectType}' could not be imported from : '{filePath}'");
                }

                RegisterFromCliAtlas(cliCommandAtlas);
            }
        }

        // added for easy chaining
        return this;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Parsing input
    // -----------------------------------------------------------------------------------------------------------------
    private bool _tryParse(IEnumerable<string> args) {
        var enumerable = args as string[] ?? args.ToArray();
        return _flagToActionMap.TryGetValue(enumerable[0].ToLower(), out var commandStruct) 
               && commandStruct.Call(enumerable[1..]); // Strip out the command and keep the arguments
    }

    private bool _tryParseMultiple(IEnumerable<string> args) {
        List<List<string>> resultLists = [];
        int startIndex = 0;
        var enumerable = args as List<string> ?? args.ToList();
        var length = enumerable.Count;

        for (int i = 0; i < length; i++) {
            if (!enumerable[i].Equals("&&")) continue;
            
            // Add the sublist from startIndex to i to the resultLists
            resultLists.Add(enumerable.GetRange(startIndex, i - startIndex));
            startIndex = i + 1;
        }

        // Add the last sublist if there's any remaining elements
        if (startIndex < length) {
            resultLists.Add(enumerable.GetRange(startIndex, length - startIndex));
        }

        return resultLists.All(_tryParse);
    }
    
    public bool TryParse(IEnumerable<string> args, bool parseMultiple) => parseMultiple ? _tryParseMultiple(args) : _tryParse(args);
    public bool TryParse(IEnumerable<string> args) => _tryParse(args);

    public void TryParseInput(bool breakOnFalse = false, bool allowMultiple = false) {
        bool breakpoint = false;

        while (!breakpoint) {
            Console.Write(Cursor);
            string[] input = Console.ReadLine()?.Split(" ") ?? [];
            bool output = allowMultiple 
                ? _tryParseMultiple(input) 
                : _tryParse(input);
                
            if (!output) {
                Console.WriteLine($"Command '{string.Join(" ", input)}' returned '{output}'");
                if (breakOnFalse) breakpoint = true;
            }
            
            Console.Write(Environment.NewLine);
        }
    }

}