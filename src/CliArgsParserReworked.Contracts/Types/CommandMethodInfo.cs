// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParserReworked.Contracts.Attributes;
using System.Reflection;

namespace CliArgsParserReworked.Contracts.Types;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ------------------------------------------------------------------------------------------------------------------
public readonly record struct CommandMethodInfo(MethodInfo info, CommandAttribute attribute, ICommandAtlas atlas) {
    public readonly MethodInfo Info = info;
    public readonly CommandAttribute CommandAttribute = attribute;

    public readonly bool IsAsync = GetIsAsync(info);
    public readonly Delegate Delegate = GetDelegate(info, attribute, atlas);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static bool GetIsAsync(MethodInfo info) =>
        info.ReturnType == typeof(Task)
        || info.ReturnType.IsGenericType
        && info.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);

    private static Delegate GetDelegate(MethodInfo info, CommandAttribute attribute, object atlas){
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
