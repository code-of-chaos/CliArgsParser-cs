// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;
using CliArgsParser.Contracts;
using CliArgsParser.Contracts.Attributes;
using CliArgsParser.PreMade.Args;
using Serilog;

namespace CliArgsParser;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

/// <summary>
/// Represents information about a command method.
/// </summary>
public readonly struct CommandMethodInfo(MethodInfo info, object atlas, ILogger logger) {
    /// <summary>
    /// Represents information about a command method.
    /// </summary>
    public readonly MethodInfo Info = info;

    /// <summary>
    /// Represents an attribute that defines a command.
    /// </summary>
    public readonly ICommandAttribute? CommandAttribute = GetCommandAttribute(info);

    /// <summary>
    /// Gets a value indicating whether the method is asynchronous.
    /// </summary>
    /// <remarks>
    /// A method is considered asynchronous if its return type is <see cref="System.Threading.Tasks.Task"/> or
    /// a generic type of <see cref="System.Threading.Tasks.Task{T}"/>.
    /// </remarks>
    /// <value><c>true</c> if the method is asynchronous; otherwise, <c>false</c>.</value>
    public readonly bool IsAsync = GetIsAsync(info);

    /// <summary>
    /// Represents a command method and its associated information.
    /// </summary>
    public readonly Delegate Delegate = GetDelegate(info, atlas, logger);

    /// <summary>
    /// Represents a parser for command-line parameters.
    /// </summary>
    public readonly IParameterParser ParameterParser = CreateParameterParser(info);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the <see cref="ICommandAttribute"/> for the specified method.
    /// </summary>
    /// <param name="info">The <see cref="MethodInfo"/> of the method.</param>
    /// <returns>The <see cref="ICommandAttribute"/> for the method, or null if not found.</returns>
    private static ICommandAttribute? GetCommandAttribute(MethodInfo info) => (ICommandAttribute?)info
        .GetCustomAttributes()
        .FirstOrDefault(a => a is ICommandAttribute, null);

    /// <summary>
    /// Determines if the given method is asynchronous.
    /// </summary>
    /// <param name="info">The MethodInfo object representing the method.</param>
    /// <returns>True if the method is asynchronous, false otherwise.</returns>
    private static bool GetIsAsync(MethodInfo info) => info.ReturnType == typeof(Task)
                                                       || (info.ReturnType.IsGenericType
                                                           && info.ReturnType.GetGenericTypeDefinition() == typeof(Task<>));

    /// <summary>
    /// Retrieves the delegate for a given method.
    /// </summary>
    /// <param name="info">The <see cref="MethodInfo"/> of the method.</param>
    /// <param name="atlas">The object instance on which the method is defined.</param>
    /// <param name="logger">The logger instance for logging.</param>
    /// <returns>The delegate for the specified method.</returns>
    public static Delegate GetDelegate(MethodInfo info, object atlas, ILogger logger) {
        Delegate commandDelegate;
        Type parameterType = GetCommandAttribute(info)?.ArgsType ?? typeof(NoArgs); // Warn quick and dirty fix
        bool isAsync = GetIsAsync(info);
        
        logger.Debug("{method} has {i} parameters",info.Name, info.GetParameters().Length);
        logger.Debug("{method} is async = {t}", info.Name, isAsync);
        try {
            switch (info.GetParameters().Length) {
                // Method is async and has parameters
                case >= 1 when isAsync && parameterType != typeof(NoArgs): {
                    Type delegateType = typeof(Func<,>).MakeGenericType(parameterType, typeof(Task));
                    commandDelegate = Delegate.CreateDelegate(delegateType, atlas, info);
                    logger.Debug("Created a delegate of type: {delegateType}", delegateType.Name);
                    break;
                }
                
                // If method is non-async action and has parameters
                case >= 1 when !isAsync && parameterType != typeof(NoArgs): {
                    Type delegateType = typeof(Action<>).MakeGenericType(parameterType);
                    commandDelegate = Delegate.CreateDelegate(delegateType, atlas, info);
                    logger.Debug("Created a delegate of type: {delegateType}", delegateType.Name);
                    break;
                }

                // If method is async action and has no parameters
                case < 1 when isAsync && parameterType == typeof(NoArgs): {
                    commandDelegate = (Func<Task>)Delegate.CreateDelegate(typeof(Func<Task>), atlas, info);
                    logger.Debug("Created a delegate of type: Func<Task>");
                    break;
                }

                case < 1 when !isAsync && parameterType == typeof(NoArgs) :
                default: {
                    commandDelegate = (Action)Delegate.CreateDelegate(typeof(Action), atlas, info);
                    logger.Debug("Created a delegate of type: Action");
                    break;
                }
                
            }
        }
        catch (Exception ex) {
            logger.Error(ex, "Failed to create delegate for method: {method}", info.Name);
            throw;
        }
        
        return commandDelegate;
    }

    /// <summary>
    /// Creates an instance of the parameter parser for a given method info.
    /// </summary>
    /// <param name="info">The method info.</param>
    /// <returns>An instance of the parameter parser.</returns>
    private static IParameterParser CreateParameterParser(MethodInfo info) {
        return new ParameterParser(GetCommandAttribute(info)?.ArgsType ?? typeof(NoArgs));
    }
}