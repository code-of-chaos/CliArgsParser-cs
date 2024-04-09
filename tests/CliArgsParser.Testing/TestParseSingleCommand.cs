// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Testing.Data;

namespace CliArgsParser.Testing;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TestParseSingleCommand(CliArgsParserFixture fixture) : IClassFixture<CliArgsParserFixture> {
    [Fact]
    public void TestAlwaysTrue() {
        Assert.True(fixture.Parser.TryParse(["always-true"]));
    }
    
    [Fact]
    public void TestAlwaysFalse() {
        Assert.False(fixture.Parser.TryParse(["always-false"]));
    }
    
    [Fact]
    public void TestNonExistingCommand() {
        Assert.False(fixture.Parser.TryParse(["i-do-not-exist"]));
    }
    
    [Fact]
    public void TestParameter() {
        string[] input = [
            "test-empty",
            "test-data -f -v data",
            "test-data-verbose --flag --value data"
        ];
        
        // Assertion is done within the commands
        foreach (string i in input) {
            bool output = fixture.Parser.TryParse(i.Split(" "));
            // All outputs have to be true
            Assert.True(output);
        }
        
    }

    [Fact]
    public void TestCliCommandEmpty() {
        string[] input = [
            "test-clicommand-empty-bool",
            "test-clicommand-empty-void",
        ];
        
        // Assertion is done within the commands
        foreach (string i in input) {
            bool output = fixture.Parser.TryParse(i.Split(" "));
            // All outputs have to be true
            Assert.True(output);
        }
    }
}