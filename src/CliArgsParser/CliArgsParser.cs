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
/// <summary>
/// The CliArgsParser class is responsible for parsing command-line arguments and executing the corresponding commands.
/// It provides methods for registering
public class CliArgsParser : ICliArgsParser {
    /// <summary>
    /// Represents a mapping between flags (command names) and actions in the CliArgsParser class.
    /// </summary>
    private readonly Dictionary<string, CommandStruct> _flagToActionMap = new();

    /// <summary>
    /// This is a dictionary that maps flag names to asynchronous action delegates.
    /// Each flag is associated with a CommandStructAsync object that contains the delegate,
    /// the command attribute,
    private readonly Dictionary<string, CommandStructAsync> _flagToActionMapAsync = new();

    /// <summary>
    /// Provides descriptions for registered command
    private static readonly Dictionary<string, string?> _desc = new();

    /// <summary>
    /// The CliArgsParser class is responsible for parsing command-line arguments and executing the corresponding commands.
    /// It provides
    public static IReadOnlyDictionary<string, string?> Descriptions =>
        _desc.AsReadOnly(); // Again added for the future, don't know what to add to it.

    /// <summary>
    /// Represents the cursor used to indicate the current input position in the command-line interface.
    /// </summary>
    public static string Cursor { get; set; } = "> ";

    /// <summary>
    /// Represents the cursor used to indicate an error in the CLI args parser.
    /// </
    public static string ErrorCursor = Cursor; // I use the same default, but you can change it

    /// <summary>
    /// Represents the delimiter used in the CliArgsParser class to separate multiple commands in a single input string
    private const string _delimiter = "&&";

    // -----------------------------------------------------------------------------------------------------------------
    // Constructor
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// A command-line arguments parser that supports registering commands and
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
    /// <summary>
    /// Registers the commands from the CliAtlas into the CliArgsParser.
    /// </summary>
    /// <typeparam name="T">The type of the CliCommandAtlas.</typeparam>
    public ICliArgsParser RegisterFromCliAtlas<T>(IEnumerable<T> cliCommandAtlas, bool force = false)
        where T : ICliCommandAtlas {
        foreach (T atlas in cliCommandAtlas) RegisterFromCliAtlas(atlas);
        return this;
    }

    /// <summary>
    /// Registers commands from a CLI command atlas.
    /// </summary>
    /// <typeparam name="T">The type of CLI
    public ICliArgsParser RegisterFromCliAtlas<T>(T cliCommandAtlas, bool overwrite = false)
        where T : ICliCommandAtlas {
        MethodInfo[] methods = cliCommandAtlas.GetType().GetMethods();

        foreach (MethodInfo methodInfo in methods) {
            // Quick exits to find the correct methods
            if (methodInfo.GetCustomAttributes().FirstOrDefault(a => a is ICliCommandAttribute) is not ICliCommandAttribute cliCommandAttribute) continue;
            
            // Find the correct delegate Type
            //      This depends on the return type & if the method has any args.
            //      Todo : add async functionality?

            if (methodInfo.ReturnType == typeof(Task)) {
                _addCommandStructAsync(methodInfo, cliCommandAttribute, cliCommandAtlas, overwrite);
            } else {
                _addCommandStruct(methodInfo, cliCommandAttribute, cliCommandAtlas, overwrite);
            }
            
        }

        // added for easy chaining
        return this;
    }

    /// Adds a command structure to the CliArgsParser.
    /// @param methodInfo The MethodInfo of the method to be associated with the command.
    /// @param cliCommandAttribute The instance of ICliCommandAttribute associated with the command
    private void _addCommandStruct(MethodInfo methodInfo, ICliCommandAttribute cliCommandAttribute, ICliCommandAtlas cliCommandAtlas, bool overwrite) {
        ParameterInfo[] methodParameters = methodInfo.GetParameters();
        bool hasArgs = methodParameters.Length != 0;
        string commandName = cliCommandAttribute.Name.ToLower();
        Type parameterType = cliCommandAttribute.ParameterOptionsType;
        Type returnType = methodInfo.ReturnType;
        Type delegateType;

        if (returnType == typeof(void)) {
            delegateType = hasArgs
                ? typeof(Action<>).MakeGenericType(parameterType)
                : typeof(Action);
        }
        else {
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
            _addException(e, methodParameters, methodInfo);
        }
        
    }

    /// <summary>
    /// Adds a command struct asynchronously to the CliArgsParser.
    /// </summary>
    /// <param name="methodInfo">The MethodInfo of the method to be added as a command.</param>
    private void _addCommandStructAsync(MethodInfo methodInfo, ICliCommandAttribute cliCommandAttribute, ICliCommandAtlas cliCommandAtlas, bool overwrite) {
        ParameterInfo[] methodParameters = methodInfo.GetParameters();
        bool hasArgs = methodParameters.Length != 0;
        string commandName = cliCommandAttribute.Name.ToLower();
        Type parameterType = cliCommandAttribute.ParameterOptionsType;
        Type delegateType;
        
        delegateType = hasArgs
            ? typeof(Func<,>).MakeGenericType(parameterType, typeof(Task))
            : typeof(Func<Task>);
        
        try {
            var del = Delegate.CreateDelegate(delegateType, cliCommandAtlas, methodInfo);
            CommandStructAsync cmdStructAsync = new (del, cliCommandAttribute, hasArgs);

            bool isAdded = _flagToActionMapAsync.TryAdd(commandName, cmdStructAsync);
            if (overwrite || isAdded) {
                if (!isAdded) _flagToActionMapAsync[commandName] = cmdStructAsync;
                    
                _desc[commandName] = cliCommandAttribute.Description;
            } 
            else if (!isAdded) {
                Console.WriteLine($"Ignoring: {commandName}");
            }
        } 
        catch (Exception e) {
            _addException(e, methodParameters, methodInfo);
        }
    }

    /// <summary>
    /// Adds an exception to the internal exception list for error handling.
    /// </summary>
    /// <param name="e">The exception to be added.</param>
    /// <param name="methodParameters">The
    private static void _addException(Exception e,  ParameterInfo[] methodParameters, MethodInfo methodInfo) {
        StringBuilder parameters = new ();
        foreach(ParameterInfo p in methodParameters) parameters.Append(p.ParameterType.FullName).Append(',');
        throw new ArgumentException($"Error attempting to bind method {methodInfo.Name}. Return Type: {methodInfo.ReturnType.Name}, Parameters: {parameters}", e);
    }

    /// <summary>
    /// Registers CLI commands from DLL files.
    /// </summary>
    /// <param name="filePaths">An IEnumerable of file paths of the DLL
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
    /// Tries to parse the input arguments and execute the corresponding command.
    /// @param args The input arguments.
    /// @return The output state
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

    /// <summary>
    /// Tries to parse the specified arguments asynchronously.
    /// </summary>
    /// <param name="args">The arguments to parse.</param>
    /// <returns>The
    private async Task<OutputState> _tryParseAsync(IEnumerable<string> args) {
        string[] enumerable = args as string[] ?? args.ToArray();

        if (_flagToActionMap.TryGetValue(enumerable[0].ToLower(), out CommandStruct commandStruct))
            return commandStruct.Call(enumerable[1..])  
                ? OutputState.True
                : OutputState.False ;

        // Trying to find entry in map of async actions
        if (_flagToActionMapAsync.TryGetValue(enumerable[0].ToLower(), out CommandStructAsync commandStructAsync)) {
            return await commandStructAsync.CallAsync(enumerable[1..]) 
                ? OutputState.True
                : OutputState.False ;
        }
        
        // else command could not be found
        Console.WriteLine($"{ErrorCursor}Command '{enumerable[0]}' not found");
        return OutputState.Undefined;
    }

    /// <summary>
    /// Splits the input arguments into separate command arrays based on the delimiter ("&&"). Each command array represents a set of arguments for a single command.
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

    /// <summary>
    /// Represents a command line arguments parser.
    /// </summary>
    private delegate Task<OutputState> ParseMultipleDelegate(IEnumerable<string> args);

    private static async Task<bool[]> HandleMultipleParsing(ParseMultipleDelegate parsingFunction,
        IEnumerable<string> args) {
        List<bool> outputBool = new();
        var foundCommands = _FindCommandInMultipleInput(args).ToArray();
    
        foreach (var currentCommand in foundCommands) {
            OutputState output = await parsingFunction(currentCommand);
            if (output == OutputState.Undefined) throw new Exception($"the command '{currentCommand}' threw an unexpected error");
            outputBool.Add(output == OutputState.True);
        }

        return outputBool.ToArray();
    }

    public bool[] TryParseMultiple(IEnumerable<string> args) {
        return HandleMultipleParsing(args => Task.FromResult(_tryParse(args)), args).Result;
    }

    /// <summary>
    /// Attempts to parse multiple command-line arguments asynchronously.
    /// </summary>
    /// <param name="args">An enumerable collection of command-line arguments.</param>
    /// <returns>A task representing the asynchronous operation
    public Task<bool[]> TryParseMultipleAsync(IEnumerable<string> args) {
        return HandleMultipleParsing(_tryParseAsync, args);
    }


    /// <summary>
    /// Tries to parse the command line arguments.
    /// </summary>
    /// <param name="args">The command line arguments to parse.</param>
    public bool TryParse(IEnumerable<string> args) => _tryParse(args) == OutputState.True;

    /// <summary>
    /// Tries to parse the given command-line arguments asynchronously.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    /// <returns>A task that represents the asynchronous operation. The task
    public async Task<bool> TryParseAsync(IEnumerable<string> args) => await _tryParseAsync(args) == OutputState.True;

    /// <summary>
    /// Prints the output message based on the output state and input command.
    /// </summary>
    /// <param name="outputState">The output state.</param>
    /// <param name="input">
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

    /// <summary>
    /// The ParseDelegate is a delegate type used for parsing command-line arguments.
    /// It takes an IEnumerable
    private delegate Task<OutputState> ParseDelegate(IEnumerable<string> args);

    /// <summary>
    /// Handles parsing of command-line arguments.
    /// </summary>
    private static async Task HandleParsing(ParseDelegate parsingFunction, bool breakOnFalse = false, bool allowMultiple = false) {
        var breakpoint = false;

        while (!breakpoint) {
            Console.Write(Cursor);
            string[] input = Console.ReadLine()?.Split(" ") ?? [];

            if (allowMultiple) {
                foreach (var currentCommand in _FindCommandInMultipleInput(input)) {
                    OutputState output = await parsingFunction(currentCommand);
                    _OutputPrint(output, input);
                    if (output == (OutputState.False | OutputState.Undefined)) breakpoint = true;
                }
            }
            else {
                _OutputPrint(await parsingFunction(input), input);
            }
        }
    }

    /// <summary>
    /// Tries to parse the input arguments.
    /// </summary>
    /// <param name="breakOnFalse">If set to <c>true</c>, stops parsing if a command returns false.
    public void TryParseInput(bool breakOnFalse = false, bool allowMultiple = false) {
        HandleParsing(args => Task.FromResult(_tryParse(args)), breakOnFalse, allowMultiple).Wait();
    }

    /// <summary>
    /// Tries to parse the input asynchronously.
    /// </summary>
    /// <param name="breakOnFalse
    public async Task TryParseInputAsync(bool breakOnFalse = false, bool allowMultiple = false) {
        await HandleParsing(_tryParseAsync, breakOnFalse, allowMultiple);
    }
}