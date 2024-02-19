// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;
using CliArgsParser.Contracts.Attributes;

namespace CliArgsParser;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a command structure that encapsulates a delegate, CLI command attribute, and flag indicating if the command has arguments.
/// </summary>
public readonly struct CommandStruct(Delegate del, ICliCommandAttribute cliCommandAttribute, bool hasArgs) {
    private Delegate Delegate { get; } = del;

    /// <summary>
    /// Gets the return type of a delegated command.
    /// </summary>
    private Type ReturnType { get; } = del.GetMethodInfo().ReturnType;

    /// <summary>
    /// Indicates whether the command has arguments.
    /// </summary>
    /// <value>
    /// <c>true</c> if the command has arguments; otherwise, <c>false</c>.
    /// </value>
    private bool HasArgs { get; } = hasArgs;

    /// <summary>
    /// Represents an attribute for CLI commands.
    /// </summary>
    private ICliCommandAttribute CliCommandAttribute { get; } = cliCommandAttribute;

    /// <summary>
    /// Executes the command associated with the given arguments.
    /// </summary>
    /// <param name="args">The arguments to parse and execute the command with.</param>
    /// <returns>
    /// Returns true if the command was executed successfully.
    /// Returns false if the command execution failed.
    /// </returns>
    public bool Call(IEnumerable<string> args) {
        // If something goes wrong here, just return false. It's a bit easier

        object? output = HasArgs
            ? Delegate.DynamicInvoke(CliCommandAttribute.GetParameters(args))
            : Delegate.DynamicInvoke() ;

        return ReturnType switch {
            { } returnType when returnType == typeof(bool) => (bool)(output ?? false),
            { } returnType when returnType == typeof(void) => true, // when the return type is null;, 
            _ => false
        };
    }

    /// <summary>
    /// Returns a string that represents the current instance.
    /// </summary>
    /// <returns>
    /// A string that represents the current instance.
    /// The returned string includes the name of the command
    /// and the delegate function.
    /// </returns>
    public override string ToString() {
        return $"{CliCommandAttribute.Name}{Environment.NewLine}{Delegate}";
    }
}