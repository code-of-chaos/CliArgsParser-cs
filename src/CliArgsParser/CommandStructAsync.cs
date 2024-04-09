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
/// Represents a command struct that can execute a delegate asynchronously.
/// </summary>
public readonly struct CommandStructAsync(Delegate del, ICliCommandAttribute cliCommandAttribute, bool hasArgs) {
    /// <summary>
    /// Represents a delegate that encapsulates a method.
    /// </summary>
    private Delegate Delegate { get; } = del;

    /// <summary>
    /// Represents the return type of a delegate used for command execution.
    /// </summary>
    private Type ReturnType { get; } = del.GetMethodInfo().ReturnType;

    /// <summary>
    /// Gets a value indicating whether the associated command has arguments.
    /// </summary>
    /// <value>
    /// <c>true</c> if the command has arguments; otherwise, <c>false</c>.
    /// </value>
    private bool HasArgs { get; } = hasArgs;

    /// <summary>
    /// Represents an attribute that decorates a method as a CLI command.
    /// </summary>
    private ICliCommandAttribute CliCommandAttribute { get; } = cliCommandAttribute;

    /// <summary>
    /// Executes the command associated with the given arguments asynchronously.
    /// </summary>
    /// <param name="args">The arguments to parse and execute the command with.</param>
    /// <returns>
    /// Returns a <see cref="Task{TResult}"/> representing the asynchronous operation.
    /// The task result is true if the command was executed successfully.
    /// The task result is false if the command execution failed.
    /// </returns>
    public async Task<bool> CallAsync(object? args = null) {
        return Delegate switch {
            Func<Task<bool>> func => await func(),
            Func<object, Task<bool>> funcWithParam when args != null => await funcWithParam(args),
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