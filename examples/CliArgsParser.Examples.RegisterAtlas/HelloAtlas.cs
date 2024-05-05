// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CliArgsParser.Attributes;

namespace CliArgsParser.Examples.RegisterAtlas;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[CommandAtlas]
public class HelloAtlas {
    
    [Command("hello")]
    public void CommmandHello() {
        Console.WriteLine("IT WORKS!");
    }
    
    [Command("hello-async")]
    public async Task CommmandHelloAsync() {
        await Task.Delay(100);
        Console.WriteLine("IT WORKS! in ASYNC");
    }
    
    [Command<ArgsTest>("hello-test")]
    public void CommmandTestArgs(ArgsTest argsTest) {
        Console.WriteLine($"IT WORKS! & with args : {argsTest.Username}");
    }
    
    [Command<ArgsTest>("hello-test-async")]
    public async Task CommmandTestArgsAsync(ArgsTest argsTest) {
        await Task.Delay(100);
        Console.WriteLine($"IT WORKS! in ASYNC & with args : {argsTest.Username}");
    }
}