// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;
using System.Text;
using CliArgsParser.Commands;
using CliArgsParser.Contracts;
using CliArgsParser.Contracts.Attributes;

namespace CliArgsParser;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CliArgsParser : ICliArgsParser {
    private readonly Dictionary<string, CommandStruct> _flagToActionMap = new();
    
    private static readonly Dictionary<string, string?> Desc = new();
    public static IReadOnlyDictionary<string, string?> Descriptions => Desc.AsReadOnly(); // Again added for the future, don't know what to add to it.

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
        foreach (T atlas in cliCommandAtlas) RegisterFromCliAtlas(atlas);
        return this;
    }
    
    public ICliArgsParser RegisterFromCliAtlas<T>(T cliCommandAtlas, bool overwrite = false) where T:ICliCommandAtlas{
        MethodInfo[] methods = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Instance);

        foreach (MethodInfo methodInfo in methods) {
            // Quick exits to find the correct methods
            if (methodInfo.GetCustomAttributes().FirstOrDefault(a => a is ICliCommandAttribute) is not ICliCommandAttribute cliCommandAttribute) continue;
            
            ParameterInfo[] methodParameters = methodInfo.GetParameters();
            bool hasArgs = methodParameters.Length != 0;
            string commandName = cliCommandAttribute.Name.ToLower();
            
            // Find the correct delegate Type
            //      This depends on the return type & if the method has any args.
            //      Todo : add async functionality?
            Type parameterType = cliCommandAttribute.ParameterOptionsType;
            Type returnType = methodInfo.ReturnType;
            Type delegateType;
            if (returnType == typeof(void)) {
                delegateType = hasArgs
                    ? typeof(Action<>).MakeGenericType(parameterType)
                    : typeof(Action);
            } else {
                delegateType = hasArgs
                    ? typeof(Func<,>).MakeGenericType(parameterType, returnType)
                    : typeof(Func<bool>);
            }
            
            try {
                var del = Delegate.CreateDelegate(delegateType, cliCommandAtlas, methodInfo);
                CommandStruct cmdStruct = new (del, cliCommandAttribute, hasArgs);
                
                bool isAdded = _flagToActionMap.TryAdd(commandName, cmdStruct);
                if (overwrite || isAdded) {
                    if (!isAdded) {
                        _flagToActionMap[commandName] = cmdStruct;
                    }
                    Desc[commandName] = cliCommandAttribute.Description;
                } 
                else if (!isAdded) {
                    Console.WriteLine($"Ignoring: {commandName}");
                }
            } 
            catch (Exception e) {
                StringBuilder parameters = new ();
                foreach(ParameterInfo p in methodParameters)
                    parameters.Append(p.ParameterType.FullName).Append(',');
                throw new ArgumentException($"Error attempting to bind method {methodInfo.Name}. Return Type: {methodInfo.ReturnType.Name}, Parameters: {parameters}", e);
            }
        }

        // added for easy chaining
        return this;
    }
    
    // TODO test this out
    public ICliArgsParser RegisterFromDlLs(IEnumerable<string> filePaths, Action? assignedCallback = null) {
        foreach (string filePath in filePaths) {
            Assembly assembly = Assembly.LoadFrom(filePath);

            foreach (Type objectType in assembly.GetTypes()) {
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
        string[] enumerable = args as string[] ?? args.ToArray();
        return _flagToActionMap.TryGetValue(enumerable[0].ToLower(), out CommandStruct commandStruct) 
               && commandStruct.Call(enumerable[1..]); // Strip out the command and keep the arguments
    }

    private IEnumerable<bool> _tryParseMultiple(IEnumerable<string> args) {
        var currentCommand = new List<string>();
        foreach (string arg in args) {
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
        var breakpoint = false;

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