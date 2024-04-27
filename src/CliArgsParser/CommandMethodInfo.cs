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
public struct CommandMethodInfo(MethodInfo info, object atlas, ILogger logger) {
    public MethodInfo Info = info;
    public ICommandAttribute? CommandAttribute { get; } = GetCommandAttribute(info);
    public bool IsAsync { get; } = GetIsAsync(info);
    public Delegate Delegate = GetDelegate(info, atlas, logger);
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
    
    public static Delegate GetDelegate(MethodInfo info, object atlas, ILogger logger) {
        Delegate commandDelegate;
        Type parameterType = GetCommandAttribute(info)?.ArgsType ?? typeof(NoArgs); // Warn quick and dirty fix
        bool isAsync = GetIsAsync(info);
        
        logger.Debug("{method} has {i} parameters",info.Name, info.GetParameters().Length);
        logger.Debug("{method} is async = {t}", info.Name, isAsync);
        try {
            switch (info.GetParameters().Length) {
                // Method is async and has parameters
                case > 1 when isAsync && parameterType != typeof(NoArgs): {
                    Type delegateType = typeof(Func<,>).MakeGenericType(parameterType, typeof(Task));
                    commandDelegate = Delegate.CreateDelegate(delegateType, atlas, info);
                    logger.Debug("Created a delegate of type: {delegateType}", delegateType.Name);
                    break;
                }
                
                // If method is non-async action and has parameters
                case > 1 when !isAsync && parameterType != typeof(NoArgs): {
                    Type delegateType = typeof(Action<>).MakeGenericType(parameterType);
                    commandDelegate = Delegate.CreateDelegate(delegateType, atlas, info);
                    logger.Debug("Created a delegate of type: {delegateType}", delegateType.Name);
                    break;
                }

                // If method is async action and has no parameters
                case < 1 when isAsync: {
                    commandDelegate = (Func<Task>)Delegate.CreateDelegate(typeof(Func<Task>), atlas, info);
                    logger.Debug("Created a delegate of type: Func<Task>");
                    break;
                }

                case < 1 when !isAsync:
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
   
    private static IParameterParser CreateParameterParser(MethodInfo info) {
        return new ParameterParser(GetCommandAttribute(info)?.ArgsType ?? typeof(NoArgs));
    }
}