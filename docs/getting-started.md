# Getting Started
A full suite of example projects can be found on the Github repo [here](https://github.com/code-of-chaos/CliArgsParser-cs/tree/core/examples)

Below are some small scale examples:

## Parsing of Program Arguments
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