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
    
    private static readonly Dictionary<string, string?> _desc = new();
    public static IReadOnlyDictionary<string, string?> Descriptions => _desc.AsReadOnly(); // Again added for the future, don't know what to add to it.

    public static string Cursor { get; set; } = "> ";
    public static string ErrorCursor = Cursor; // I use the same default, but you can change it
    private const string _delimiter = "&&";
    
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
        MethodInfo[] methods = cliCommandAtlas.GetType().GetMethods();
        
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
                    if (!isAdded) _flagToActionMap[commandName] = cmdStruct;
                    
                    _desc[commandName] = cliCommandAttribute.Description;
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
                if (!objectType.IsAssignableTo(typeof(ICliCommandAtlas))) continue;
                if (objectType is { IsInterface: true} or {IsAbstract: true }) continue;
                var cliCommandAtlas = (ICliCommandAtlas)Activator.CreateInstance(objectType)!;
                
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
    private OutputState _tryParse(IEnumerable<string> args) {
        string[] enumerable = args as string[] ?? args.ToArray();

        if (_flagToActionMap.TryGetValue(enumerable[0].ToLower(), out CommandStruct commandStruct))
            
            return commandStruct.Call(enumerable[1..])  // Strip out the command and keep the arguments
                ? OutputState.True
                : OutputState.False ;
        
        // else command could not be found
        Console.WriteLine($"{ErrorCursor}Command '{enumerable[0]}' not found");
        return OutputState.Undefined;
    }

    private static IEnumerable<string[]> _FindCommandInMultipleInput(IEnumerable<string> args) {
        var currentCommand = new List<string>();
        foreach (string arg in args) {
            if (arg.Equals(_delimiter)) {
                yield return currentCommand.ToArray();
                currentCommand.Clear();
            } 
            else {
                currentCommand.Add(arg);
            }
        }
        if (currentCommand.Count != 0) {
            yield return currentCommand.ToArray();
        }
    }
    
    public bool[] TryParseMultiple(IEnumerable<string> args) {
        List<bool> outputBool = [];
        var foundCommands = _FindCommandInMultipleInput(args).ToArray();
        
        foreach (var currentCommand in foundCommands) {
            OutputState output = _tryParse(currentCommand);
            if (output == OutputState.Undefined) throw new Exception($"the command '{currentCommand}' threw an unexpected error");
            outputBool.Add(output == OutputState.True);

        }
        return outputBool.ToArray() ;
    }
    
    public bool TryParse(IEnumerable<string> args) => _tryParse(args) == OutputState.True;
    
    private static void _OutputPrint(OutputState outputState, IEnumerable<string> input) {
        Console.WriteLine(
            outputState switch {
                OutputState.Undefined => $"{ErrorCursor}Invalid input",
                OutputState.False => $"{ErrorCursor}Command '{string.Join(" ", input)}' returned False",
                OutputState.True => Environment.NewLine,
                _ => throw new ArgumentOutOfRangeException() // this should never happen
            }
        );
    }
    
    public void TryParseInput(bool breakOnFalse = false, bool allowMultiple = false) {
        var breakpoint = false;

        while (!breakpoint) {
            Console.Write(Cursor);
            string[] input = Console.ReadLine()?.Split(" ") ?? [];

            if (allowMultiple) {
                foreach (var currentCommand in _FindCommandInMultipleInput(input)) {
                    OutputState output =_tryParse(currentCommand);
                    _OutputPrint(output, input);
                    if (output == (OutputState.False | OutputState.Undefined)) breakpoint = true;
                }
            }
            else {
                _OutputPrint(_tryParse(input), input);
            }
        }
    }
}