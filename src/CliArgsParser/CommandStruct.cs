// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Contracts.Attributes;

namespace CliArgsParser;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct CommandStruct(Delegate del, ICliCommandAttribute cliCommandAttribute) {
    private Delegate Delegate { get; } = del;
    private ICliCommandAttribute CliCommandAttribute { get; } = cliCommandAttribute;

    public bool Call(IEnumerable<string> args) {
        // If something goes wrong here, just return false. It's a bit easier
        return (bool)(Delegate.DynamicInvoke(CliCommandAttribute.GetParameters(args)) ?? false);
    }

    public override string ToString() {
        return $"{CliCommandAttribute.Name}{Environment.NewLine}{Delegate}";
    }
}