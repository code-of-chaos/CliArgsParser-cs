// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;
using CliArgsParser.Contracts.Attributes;

namespace CliArgsParser;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct CommandStruct(Delegate del, ICliCommandAttribute cliCommandAttribute, bool hasArgs) {
    private Delegate Delegate { get; } = del;
    private Type ReturnType { get; } = del.GetMethodInfo().ReturnType;
    private bool HasArgs { get; } = hasArgs;
    private ICliCommandAttribute CliCommandAttribute { get; } = cliCommandAttribute;

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

    public override string ToString() {
        return $"{CliCommandAttribute.Name}{Environment.NewLine}{Delegate}";
    }
}