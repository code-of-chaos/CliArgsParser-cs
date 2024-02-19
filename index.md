---
_layout: landing
title: CliArgsParser
---

# Welcome to CliArgsParser

CliArgsParser brings the power of simplicity to your command-line tool development.
Craft your command-line interfaces and parse program arguments with ease. Whether you're quickly prototyping a new tool or developing an enterprise-level application, embrace the flexibility and power of CliArgsParser.

## Features

- **Effortless Argument Parsing:** With intuitive API, start parsing command-line arguments in no time.
- **Robust Validation:** Ensure input correctness with built-in and customizable validation mechanisms.
- **High Flexibility:** Crafting your ideal command-line interface is a breeze with configurable options.

## API Documentation

Navigate to our [API Documentation](./api) for in-depth understanding of our libraries.

## Getting Started

Check out our [Examples](https://github.com/code-of-chaos/CliArgsParser-cs/tree/core/examples) for some examples.

Or look below for a small scale example.

### Parsing of Program Arguments
```csharp
using CliArgsParser;
using CliArgsParser.Attributes;

public class ArgOptions : ParameterOptions {
    [ArgValue('u', "username")]  public string? Username { get; set; }
}

public class CommandAtlas : CliCommandAtlas {
    [CliCommand<ArgOptions>("test")]
    public void CallbackTest(ArgOptions argOptions) {
        Console.WriteLine(argOptions.Username != null ? 
            $"Hello, {argOptions.Username}!" : 
            "Hello, stranger"
        );
    }
}

static class Program {
    public static void Main(string[] args) {
        new CliArgsParser.CliArgsParser()
            .RegisterFromCliAtlas(new CommandAtlas())
            .TryParseMultiple(args);
    }
}
```

| input             | output            |
|-------------------|-------------------|
| `test`            | `Hello, stranger` |
| `test -u Andreas` | `Hello, Andreas`  |