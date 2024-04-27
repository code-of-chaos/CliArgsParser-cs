// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;
using CliArgsParser.Contracts;
using CliArgsParser.Contracts.Attributes;
using CliArgsParser.PreMade.Args;

namespace CliArgsParser;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public struct CommandMethodInfo(MethodInfo info) {
    public MethodInfo Info = info;
    public ICommandAttribute? CommandAttribute { get; } = GetCommandAttribute(info);
    public bool IsAsync { get; } = GetIsAsync(info);
    public Delegate Delegate = GetDelegate(info);
    public IParameterParser ParameterParser = CreateParameterParser(info);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static ICommandAttribute? GetCommandAttribute(MethodInfo info) => (ICommandAttribute?)info
        .GetCustomAttributes()
        .FirstOrDefault(a => a is ICommandAttribute, null);
    
    private static bool GetIsAsync(MethodInfo info) => info.ReturnType == typeof(Task)
                                                       || (info.ReturnType.IsGenericType
                                                           && info.ReturnType.GetGenericTypeDefinition() == typeof(Task<>));
    
    public static Delegate GetDelegate(MethodInfo info) {
        Delegate commandDelegate;
        Type parameterType = GetCommandAttribute(info)?.ArgsType ?? typeof(NoArgs); // Warn quick and dirty fix
        bool isAsync = GetIsAsync(info);
        
        switch (info.GetParameters().Length) {
            // Method is async and has parameters
            case > 0 when isAsync: {
                Type delegateType = typeof(Func<,>).MakeGenericType(parameterType, typeof(Task));
                commandDelegate = Delegate.CreateDelegate(delegateType, null, info);
                break;
            }
            
            // If method is non-async action and has parameters
            case > 0 when !isAsync: {
                Type delegateType = typeof(Action<>).MakeGenericType(parameterType);
                commandDelegate = Delegate.CreateDelegate(delegateType, null, info);
                break;
            }

            // If method is async action and has no parameters
            case 0 when isAsync: {
                commandDelegate = (Func<Task>)Delegate.CreateDelegate(typeof(Func<Task>), null, info);
                break;
            }
            
            // Method is non-async action and has no parameters
            default: {
                commandDelegate = (Action)Delegate.CreateDelegate(typeof(Action), null, info);
                break;
            }
        }

        return commandDelegate;
    }
   
    private static IParameterParser CreateParameterParser(MethodInfo info) {
        return new ParameterParser(GetCommandAttribute(info)?.ArgsType ?? typeof(NoArgs));
    }
}