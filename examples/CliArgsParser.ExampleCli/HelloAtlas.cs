// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Contracts;
using CliArgsParser.Contracts.Attributes;

namespace CliArgsParser.ExampleCli;
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

    [Command<ArgsTest>("hello-args")]
    public void CommandTestArgs(ArgsTest argsTest) {
        Console.WriteLine($"IT WORKS! & with args : {argsTest.Username}");
    }

    [Command<ArgsTest>("hello-args-async")]
    public async Task CommandTestArgsAsync(ArgsTest argsTest) {
        await Task.Delay(100);
        Console.WriteLine($"IT WORKS! in ASYNC & with args : {argsTest.Username}");
    }
}
