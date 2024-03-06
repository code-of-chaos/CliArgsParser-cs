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
/// It provides methods for registering commands, parsing arguments, and handling input.
/// </summary>
public class CliArgsParser : ICliArgsParser {
    private readonly Dictionary<string, CommandStruct> _flagToActionMap = new();
    private static readonly Dictionary<string, string?> _desc = new();

    /// <summary>
    /// The CliArgsParser class is responsible for parsing command-line arguments and executing the corresponding commands.
    /// It provides methods for registering commands, parsing arguments, and handling input.
    /// </summary>
    public static IReadOnlyDictionary<string, string?> Descriptions => _desc.AsReadOnly(); // Again added for the future, don't know what to add to it.

    /// <summary>
    /// Represents the cursor used in the command line interface.
    /// </summary>
    public static string Cursor { get; set; } = "> ";

    /// <summary>
    /// Represents the cursor used to indicate an error in the CLI args parser.
    /// </summary>
    public static string ErrorCursor = Cursor; // I use the same default, but you can change it

    /// <summary>
    /// Represents the delimiter used in the CliArgsParser class to separate multiple commands in a single input string.
    /// </summary>
    private const string _delimiter = "&&";
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructor
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// A command-line arguments parser that supports registering commands and parsing input.
    /// </summary>
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
    /// <param name="cliCommandAtlas">The CliCommandAtlas containing the commands to register.</param>
    /// <param name="force">A flag indicating whether to overwrite existing commands with the same name.</param>
    /// <returns>The instance of the CliArgsParser.</returns>
    public ICliArgsParser RegisterFromCliAtlas<T>(IEnumerable<T> cliCommandAtlas, bool force = false) where T : ICliCommandAtlas {
        foreach (T atlas in cliCommandAtlas) RegisterFromCliAtlas(atlas);
        return this;
    }

    /// <summary>
    /// Registers commands from a CLI command atlas.
    /// </summary>
    /// <typeparam name="T">The type of CLI command atlas.</typeparam>
    /// <param name="cliCommandAtlas">The CLI command atlas.</param>
    /// <param name="overwrite">Flag indicating whether to overwrite existing commands with the same name.</param>
    /// <returns>The instance of the ICliArgsParser interface.</returns>
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
    
    /// <summary>
    /// Registers CLI commands from DLL files.
    /// </summary>
    /// <param name="filePaths">An IEnumerable of file paths of the DLL files.</param>
    /// <param name="assignedCallback">An optional callback action to be invoked after each command is registered.</param>
    /// <returns>The instance of the <see cref="CliArgsParser"/> class to allow method chaining.</returns>
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
    /// <summary>
    /// Tries to parse the input arguments and execute the corresponding command.
    /// </summary>
    /// <param name="args">The input arguments.</param>
    /// <returns>The output state of the command execution.</returns>
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
    /// Splits the input arguments into separate command arrays based on the delimiter (&quot;&amp;&amp;&quot;). Each command array represents a set of arguments for a single command.
    /// </summary>
    /// <param name="args">The input arguments.</param>
    /// <returns>An enumerable of string arrays, where each array represents a command.</returns>
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
    /// Parses multiple command line arguments and returns a list of boolean values indicating the success of each command.
    /// </summary>
    /// <param name="args">The command line arguments to parse.</param>
    /// <returns>A list of boolean values indicating the success of each command.</returns>
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

    /// <summary>
    /// Tries to parse the command line arguments.
    /// </summary>
    /// <param name="args">The command line arguments to parse.</param>
    /// <returns>True if the parsing was successful, otherwise false.</returns>
    public bool TryParse(IEnumerable<string> args) => _tryParse(args) == OutputState.True;

    /// <summary>
    /// Prints the output message based on the output state and input command.
    /// </summary>
    /// <param name="outputState">The output state.</param>
    /// <param name="input">The input command.</param>
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
    /// Parses user input from the console and attempts to parse the input as command line arguments.
    /// </summary>
    /// <param name="breakOnFalse">A flag indicating whether to break out of the loop if parsing returns a False result.</param>
    /// <param name="allowMultiple">A flag indicating whether to allow parsing multiple commands in a single input.</param>
    /// <remarks>
    /// This method continuously prompts the user for input using the specified cursor.
    /// Each input is split into individual arguments and passed to the TryParse method.
    /// If allowMultiple is true, the method will attempt to parse each individual command and output the result.
    /// If allowMultiple is false, the method will attempt to parse the entire input as a single command.
    /// The loop continues until a breakpoint is reached or the program is terminated.
    /// </remarks>
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