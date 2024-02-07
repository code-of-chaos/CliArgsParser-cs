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
        IEnumerable<bool>? output = fixture.Parser.TryParseMultiple(input.Split(" "));

        Assert.All(output, Assert.True);
    }
    
    [Fact]
    public void TestAlwaysFalse() {
        const string input = "always-false && always-false";
        IEnumerable<bool>? output = fixture.Parser.TryParseMultiple(input.Split(" "));
        
        Assert.All(output, Assert.False);
    }
    
    [Fact]
    public void TestData() {
        string[] input = [
            "test-empty && test-data -f -v data && test-data-verbose --flag --value data",
        ];
        
        foreach (string i in input) {
            IEnumerable<bool>? output = fixture.Parser.TryParseMultiple(i.Split(" "));
            Assert.All(output, Assert.True);
        }
        
    }
}