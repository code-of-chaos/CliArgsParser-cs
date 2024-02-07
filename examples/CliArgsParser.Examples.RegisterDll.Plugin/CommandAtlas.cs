// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Attributes;
using CliArgsParser.Commands;

namespace CliArgsParser.Examples.RegisterDll.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable UnusedType.Global
public class CommandAtlas : CliCommandAtlas {
    // ReSharper restore UnusedType.Global
    [CliCommand<NoArgs>("plugin")]
    public void CallbackPluginCommand() {
        Console.WriteLine("This command is imported from another dll file");
    }
}