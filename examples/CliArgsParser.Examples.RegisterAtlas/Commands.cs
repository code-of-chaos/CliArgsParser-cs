// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Attributes;

namespace CliArgsParser.Examples.RegisterAtlas;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Commands : CliCommandAtlas {

    private Dictionary<string, int> _usersTested = new();
    
    public Commands() {
        _usersTested["Andreas"] = 0;
    }
    
    [CliCommand<ArgOptions>("test")]
    public bool CallbackTest(ArgOptions argOptions) {
        if (argOptions.Username == null) {
            Console.WriteLine("Hello Stranger");
        } 
        
        if (!_usersTested.TryAdd(argOptions.Username!, 1)) {
            _usersTested[argOptions.Username!]++;
        }

        Console.WriteLine($"Hello, {argOptions.Username}!");
        return true;
    }
}