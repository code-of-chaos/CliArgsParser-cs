// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Testing.Data;
using Xunit.Abstractions;

namespace CliArgsParser.Testing;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TestParseAsync : IClassFixture<CliArgsParserFixture> {
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly CliArgsParserFixture _fixture;
    public TestParseAsync(ITestOutputHelper testOutputHelper, CliArgsParserFixture fixture) {
        _testOutputHelper = testOutputHelper;
        this._fixture = fixture;
    }

    [Fact]
    public async Task TestCliCommandAsync() {
        string[] input = [
            "test-clicommand-async-true"
        ];
        
        // Assertion is done within the commands
        foreach (string i in input) {
            bool output = await _fixture.Parser.TryParseAsync(i.Split(" "));
            _testOutputHelper.WriteLine(output.ToString());
            // All outputs have to be true
            Assert.True(output);
        }
    }
    
    [Fact]
    public async Task TestCliCommandAsyncFalse() {
        string[] input = [
            "test-clicommand-async-false"
        ];
        
        // Assertion is done within the commands
        foreach (string i in input) {
            bool output = await _fixture.Parser.TryParseAsync(i.Split(" "));
            // All outputs have to be true
            Assert.False(output);
        }
    }
    
    [Fact]
    public async Task TestCliCommandAsyncSpore() {
        string[] input = [
            "test-clicommand-async-spore"
        ];
        
        // Assertion is done within the commands
        foreach (string i in input) {
            string output = await _fixture.Parser.TryParseAsync(i.Split(" "));
            // All outputs have to be true
            Assert.True(output == "stats");
        }
    }
}
