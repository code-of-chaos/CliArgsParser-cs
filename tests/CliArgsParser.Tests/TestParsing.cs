// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CliArgsParser.Tests.Data;

namespace CliArgsParser.Tests;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TestParsing(ArgsParserFixture fixture) : IClassFixture<ArgsParserFixture> {
     
    [Theory]
    [InlineData("test-sync")]
    [InlineData("test-sync-params-empty")]
    public void TestSync(string input) {
        Assert.Throws<SuccessException>(() => fixture.Parser.TryParse(input));
    }
    
    [Theory]
    [InlineData("test-async")]
    [InlineData("test-async-params-empty")]
    public async Task TestAsync(string input) {
        await Assert.ThrowsAsync<SuccessException>(async () => await fixture.Parser.TryParseAsync(input));
    }
}