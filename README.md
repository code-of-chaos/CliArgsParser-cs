# CLI Args Parser
CLI Args Parser is a library which allows you to process command line arguments in an efficient and structured way. 
This is particularly useful in scenarios where your application has numerous command line options, or where commands themselves are complex and require arguments.

## Features
Here are the key features of CLI Args Parser:
- **Command Declaration and Registration**: Commands are declaratively defined within a class using the Command attribute. The class containing these commands is marked with the CommandAtlas attribute. This arrangement provides a well-structured, easy-to-maintain way of defining and locating command handling logic.
- **Asynchronous Command Execution**: CLI Args Parser allows for asynchronous execution of commands. This can provide major performance benefits, especially when commands involve IO-bound operations.
- **Argument Mapping**: CLI Args Parser allows property-based argument mapping. Properties of a class implementing the IParameters interface can be used as the target of argument mapping from command line input. Argument mapping is further simplified through the use of the AutoArgValue attribute which takes the argument name as a parameter.
- **Integration with Serilog**: Logging is a vital aspect, especially in debugging scenarios. CLI Args Parser is designed with integration hooks for Serilog, allowing for robust logging during command and argument parsing. Default is set to `Logger.None` so you can enable the logger when needed.

## Quick Start
Here's a basic example on how to get started with CLI Args Parser:

```csharp
using CliArgsParser.Attributes;
using CliArgsParser.Contracts;

// Define a class for mapping arguments
public class ArgsTest : IParameters  {
    [AutoArgValue("name")]
    public string Name { get; set; } = "Default Name";
}

[CommandAtlas]
public class MyAppCommands{
    
    [Command("hello")]
    public void CommandHello() {
        Console.Writeline("Hello there!");    
        /* Or your command' implementation, without arguments, goes here... */
    }

    [Command<ArgsTest>("greet")]
    public void CommandGreet(ArgsTest argsTest)  {
        /* Or Your command implementation goes here... */
        Console.Writeline($"Hello there and welcome, {argsTest.Name}");  
    }
}

static class Program {    
    public static void Main(string[] args) {
        IParser parser = new ParserConfiguration()
            .RegisterAtlas(new MyAppCommands())
            .CreateArgsParser();
        
        parser.TryParse("greet --name=John");
    }
}
```