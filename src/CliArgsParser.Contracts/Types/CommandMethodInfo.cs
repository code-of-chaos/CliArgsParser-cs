// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;

// ReSharper disable CheckNamespace
namespace CliArgsParser;
// ReSharper restore CheckNamespace
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents information about a command method in a command-line interface (CLI) application.
/// </summary>
/// <param name="Info">The <see cref="MethodInfo"/> of the method being described.</param>
/// <param name="CommandAttribute">The <see cref="CommandAttribute"/> associated with the method.</param>
/// <param name="CommandAtlas">The instance of <see cref="ICommandAtlas"/> that contains the method.</param>
/// <param name="DescriptionAttribute">An optional <see cref="DescriptionAttribute"/> providing a description of the method.</param>
public readonly record struct CommandMethodInfo(
    MethodInfo Info, 
    CommandAttribute CommandAttribute, 
    ICommandAtlas CommandAtlas, 
    DescriptionAttribute? DescriptionAttribute = null
) {
    /// <summary>
    /// The type of the method's parameter as specified by the <see cref="CommandAttribute.ArgsType"/>.
    /// </summary>
    public readonly Type ParameterType = CommandAttribute.ArgsType;

    /// <summary>
    /// Indicates whether the method is asynchronous.
    /// </summary>
    public readonly bool IsAsync = GetIsAsync(Info);
    
    /// <summary>
    /// The delegate associated with the command method, allowing it to be invoked dynamically.
    /// </summary>
    public readonly Delegate Delegate = GetDelegate(Info, CommandAttribute, CommandAtlas);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Retrieves whether or no the method is Async.
    /// </summary>
    private static bool GetIsAsync(MethodInfo info) =>
        info.ReturnType == typeof(Task)
        || info.ReturnType.IsGenericType
        && info.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);

    /// <summary>
    /// Creates the delegate associated with the given method.
    /// </summary>
    /// <param name="info">The <see cref="MethodInfo"/> of the method.</param>
    /// <param name="attribute">The <see cref="CommandAttribute"/> of the method.</param>
    /// <param name="atlas">The object that contains the method.</param>
    /// <returns>The <see cref="System.Delegate"/> associated with the method.</returns>
    private static Delegate GetDelegate(MethodInfo info, CommandAttribute attribute, object atlas) {
        Delegate commandDelegate;
        Type parameterType = attribute.ArgsType;
        bool isAsync = GetIsAsync(info);

        switch (info.GetParameters().Length) {
            // Method is async and has parameters
            case >= 1 when isAsync && parameterType != typeof(NoArgs): {
                Type delegateType = typeof(Func<,>).MakeGenericType(parameterType, typeof(Task));
                commandDelegate = Delegate.CreateDelegate(delegateType, atlas, info);
                break;
            }

            // If method is non-async action and has parameters
            case >= 1 when !isAsync && parameterType != typeof(NoArgs): {
                Type delegateType = typeof(Action<>).MakeGenericType(parameterType);
                commandDelegate = Delegate.CreateDelegate(delegateType, atlas, info);
                break;
            }

            // If method is async action and has no parameters
            case < 1 when isAsync && parameterType == typeof(NoArgs): {
                commandDelegate = (Func<Task>)Delegate.CreateDelegate(typeof(Func<Task>), atlas, info);
                break;
            }

            case < 1 when !isAsync && parameterType == typeof(NoArgs):
            default: {
                commandDelegate = (Action)Delegate.CreateDelegate(typeof(Action), atlas, info);
                break;
            }

        }

        return commandDelegate;
    }
}
