// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Testing.Data;

namespace CliArgsParser.Testing;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TestParseMultipleCommands(CliArgsParserFixture fixture) : IClassFixture<CliArgsParserFixture> {
     
    [Fact]
    public void TestAlwaysTrue() {
        const string input = "always-true && always-true";
        var output = fixture.Parser.TryParseMultiple(input.Split(" "));

        Assert.All(output, Assert.True);
    }
    
    [Fact]
    public void TestAlwaysFalse() {
        const string input = "always-false && always-false";
        var output = fixture.Parser.TryParseMultiple(input.Split(" "));
        
        Assert.All(output, Assert.False);
    }
}