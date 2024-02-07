// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;
using System.Text;
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

    public static string Cursor { get; set; } = "> ";
    public static string ErrorCursor = Cursor; // I use the same default, but you can change it
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructor
    // -----------------------------------------------------------------------------------------------------------------
    public CliArgsParser(string cursor = "> ", bool addDefault = true) {
        // There is two default commands "HELP", which lists all command, and "EXIT", which exists when in input mode
        //      Though I have now added this as an optional
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
    
    public ICliArgsParser RegisterFromCliAtlas<T>(T cliCommandAtlas, bool overwrite = false) where T:ICliCommandAtlas{
        var methods = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Instance);

        foreach (var methodInfo in methods) {
            // Quick exits to find the correct methods
            if (methodInfo.GetCustomAttributes().FirstOrDefault(a => a is ICliCommandAttribute) is not ICliCommandAttribute cliCommandAttribute) continue;
            
            var methodParameters = methodInfo.GetParameters();
            bool hasArgs = methodParameters.Length != 0;
            string commandName = cliCommandAttribute.Name.ToLower();
            
            // Find the correct delegate Type
            //      This depends on the return type & if the method has any args.
            //      Added the void & no args options for ease of use
            Type delegateType = methodInfo.ReturnType switch {
                { } returnType when returnType == typeof(bool) => hasArgs
                    ? typeof(CmdCallback<>).MakeGenericType(cliCommandAttribute.ParameterOptionsType)
                    : typeof(CmdCallback),

                { } returnType when returnType == typeof(void) => hasArgs
                    ? typeof(CmdCallbackVoid<>).MakeGenericType(cliCommandAttribute.ParameterOptionsType)
                    : typeof(CmdCallbackVoid),
    
                _ => throw new Exception("DelegateType could not be created")
            };
            
            try {
                Delegate del = Delegate.CreateDelegate(delegateType, cliCommandAtlas, methodInfo);
                CommandStruct cmdStruct = new CommandStruct(del, cliCommandAttribute, hasArgs);
                
                bool isAdded = _flagToActionMap.TryAdd(commandName, cmdStruct);
                if (overwrite || isAdded) {
                    if (!isAdded) {
                        _flagToActionMap[commandName] = cmdStruct;
                    }
                    _descriptions[commandName] = cliCommandAttribute.Description;
                } 
                else if (!isAdded) {
                    Console.WriteLine($"Ignoring: {commandName}");
                }
            } 
            catch (Exception e) {
                StringBuilder parameters = new StringBuilder();
                foreach(var p in methodParameters)
                    parameters.Append(p.ParameterType.FullName).Append(',');
                throw new ArgumentException($"Error attempting to bind method {methodInfo.Name}. Return Type: {methodInfo.ReturnType.Name}, Parameters: {parameters}", e);
            }
        }

        // added for easy chaining
        return this;
    }
    
    // TODO test this out
    public ICliArgsParser RegisterFromDlLs(IEnumerable<string> filePaths, Action? assignedCallback = null) {
        foreach (var filePath in filePaths) {
            Assembly assembly = Assembly.LoadFrom(filePath);

            foreach (var objectType in assembly.GetTypes()) {
                if (!typeof(ICliCommandAtlas).IsAssignableFrom(objectType)) continue;
                if (objectType is { IsInterface: true, IsAbstract: true }) continue;
                if (Activator.CreateInstance(objectType) is not ICliCommandAtlas cliCommandAtlas) continue;
                // throw new Exception($"Command atlas '{objectType}' could not be imported correctly from : '{filePath}'");
                
                // Actually register the commands
                RegisterFromCliAtlas(cliCommandAtlas);
                assignedCallback?.Invoke();
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

    private IEnumerable<bool> _tryParseMultiple(IEnumerable<string> args) {
        var currentCommand = new List<string>();
        foreach (var arg in args) {
            if (arg.Equals("&&")) {
                yield return _tryParse(currentCommand);
                currentCommand.Clear();
            } else {
                currentCommand.Add(arg);
            }
        }
        if (currentCommand.Count != 0) {
            yield return _tryParse(currentCommand);
        }
    }
    
    public IEnumerable<bool> TryParseMultiple(IEnumerable<string> args) => _tryParseMultiple(args);
    public bool TryParse(IEnumerable<string> args) => _tryParse(args);
    

    public void TryParseInput(bool breakOnFalse = false, bool allowMultiple = false) {
        bool breakpoint = false;

        while (!breakpoint) {
            Console.Write(Cursor);
            string[] input = Console.ReadLine()?.Split(" ") ?? [];
            bool output = allowMultiple 
                ? _tryParseMultiple(input).All(a => a) 
                : _tryParse(input);
                
            if (!output) {
                Console.WriteLine($"{ErrorCursor}Command '{string.Join(" ", input)}' returned '{output}'");
                if (breakOnFalse) breakpoint = true;
            }
            
            Console.Write(Environment.NewLine);
        }
    }

}