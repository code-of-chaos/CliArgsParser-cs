// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParserReworked.Contracts;
using CliArgsParserReworked.Contracts.Attributes;

namespace CliArgsParserReworked.Example;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class HelloAtlas : ICommandAtlas {
    [Command("hello")]
    public void CommandHello() {
        Console.WriteLine("IT WORKS!");
    }

    [Command("hello-async")]
    public async Task CommandHelloAsync() {
        await Task.Delay(100);
        Console.WriteLine("IT WORKS! in ASYNC");
    }

    [Command<ArgsTest>("hello-test")]
    public void CommandTestArgs(ArgsTest argsTest) {
        Console.WriteLine($"IT WORKS! & with args : {argsTest.Username}");
    }

    [Command<ArgsTest>("hello-test-async")]
    public async Task CommandTestArgsAsync(ArgsTest argsTest) {
        await Task.Delay(100);
        Console.WriteLine($"IT WORKS! in ASYNC & with args : {argsTest.Username}");
    }
}
