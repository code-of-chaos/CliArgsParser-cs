// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CliArgsParser.Examples.RegisterDll;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
static class Program {
    public static void Main(string[] args) {
        const  string cliPluginsFolder = @"plugins";
        
        new CliArgsParser()
            .RegisterFromDlLs(Directory.GetFiles(cliPluginsFolder, "*.dll"))
            .TryParseMultiple(args);
    }
}