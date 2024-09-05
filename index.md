# CliArgsParser
![NuGet Downloads](https://img.shields.io/nuget/dt/CliArgsParser)
![NuGet Version](https://img.shields.io/nuget/v/CliArgsParser?link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FCliArgsParser)

CliArgsParser is a library built around Dependency Injection to allow you to create Cli tools with ease.
This is particularly useful in scenarios where your application requires a large amount of commands,
with specific arguments.

```shell
dotnet add package CliArgsParser --version 2.2.7
```

## Features
Here are the key features of Cli Args Parser:
- **Command Declaration and Registration**: Commands are declaratively defined within a class using the `ICommandAtlas` interface.
  This arrangement provides a well-structured, easy-to-maintain way of defining and locating command handling logic.
- **Asynchronous Command Execution**: CliArgsParser allows for asynchronous execution of commands.
  This can provide major performance benefits, especially when commands involve IO-bound operations.
  The library also exposes the `ArgsParser.ParseAsyncParallel` method for parallel async execution of cli tools.
- **Argument Mapping**: CliArgsParser allows property-based argument mapping.
  Properties of a class implementing the ICommandParameters interface can be used as the target of argument mapping from command line input.
  Argument mapping is further simplified through the use of the `ArgValue` and `ArgFlag` attributes which takes the argument name as a parameter.
- **Dependency Injection approach**: The library is built around using DI in your workflow. `ICommandAtlas` classes allow constructors to fully utilize dependency injection.
  Yet you are not required to have a `IServiceCollection`, by using the `CliParser.CreateStandalone()` or `ArgsParser.CreateStandalone()` methods.

## F.A.Q:

> **"I tried to create a atlas with async commands, but they did not execute when called"**
>
> This is due to the parser trying to execute the commands as non-async, because the parser was most likely called without using the async methods.
> To resolve this, always run the parsers through their async methods.

## Quick Start
The following section is a basic example on how to get started with CliArgsParser.
For in-code examples, they can be found in the repo under examples : https://github.com/code-of-chaos/CliArgsParser-cs/tree/core/examples

### Dependency Injected approach
```csharp
using CliArgsParser;

// Define a class for mapping arguments
public class ArgsTest : ICommandParameters  {
    [ArgValue("name"), Description("Defines the name of the user")] 
    public string Name { get; set; } = "Default Name";
}

public class MyAppCommands : ICommandAtlas {
    [Command("hello"), Description("Greets everyone")]
    public void CommandHello() {
        Console.Writeline("Hello there!");    
        /* Or your command' implementation, without arguments, goes here... */
    }

    [Command<ArgsTest>("greet"), Description("Greets you specificly")]
    public void CommandGreet(ArgsTest argsTest)  {
        Console.Writeline($"Hello there and welcome, {argsTest.Name}");  
        /* Or Your command implementation goes here... */
    }
}

internal static class Program {
    public async static Task Main(string[] args) {
        IServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCliArgsParser(configuration =>
            configuration
                .SetConfig(new CliArgsParserConfig {
                    Overridable = true,
                    GenerateShortNames = true
                })
                .AddFromType<HelloAtlas>()
        );
        
        ServiceProvider provider = serviceCollection.BuildServiceProvider();
        
        var cliParser =  provider.GetRequiredService<ICliParser>();
        await cliParser.StartParsingAsync();
    }
}
```

### Standalone Approach
```csharp
using CliArgsParser;

// Define a class for mapping arguments
public class ArgsTest : ICommandParameters  {
    [ArgValue("name"), Description("Defines the name of the user")] 
    public string Name { get; set; } = "Default Name";
}

public class MyAppCommands : ICommandAtlas {
    [Command("hello"), Description("Greets everyone")]
    public void CommandHello() {
        Console.Writeline("Hello there!");    
        /* Or your command' implementation, without arguments, goes here... */
    }

    [Command<ArgsTest>("greet"), Description("Greets you specificly")]
    public void CommandGreet(ArgsTest argsTest)  {
        Console.Writeline($"Hello there and welcome, {argsTest.Name}");  
        /* Or Your command implementation goes here... */
    }
}

internal static class Program {
    public async static Task Main(string[] args) {
        ICliParser parser = CliParser.CreateStandalone(
            configuration =>
                configuration
                    .SetConfig(new CliArgsParserConfig {
                        Overridable = true,
                        GenerateShortNames = true
                    })
                    .AddFromType<HelloAtlas>()
        );
        await cliParser.StartParsingAsync();
    }
}
```