// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Tests.Data.Fixture;

namespace CliArgsParser.Tests;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TestParsing(ArgsParserFixture fixture) : IClassFixture<ArgsParserFixture> {
     
    [Theory]
    [InlineData("test-sync", "defined", null, null)]
    [InlineData("test-sync-params-empty", "empty", null, false)]
    public void TestSync(string input, string? expectedSomeData, string? expectedArgsValue, bool? expectedArgsFlag) {
        
        fixture.Parser.TryParse(input);
        
        Assert.Equal(expectedSomeData, fixture.DataOutput.SomeData);
        Assert.Equal(expectedArgsValue, fixture.DataOutput.ArgsValue);
        Assert.Equal(expectedArgsFlag, fixture.DataOutput.ArgsFlag);
    }
    
    [Theory]
    [InlineData("test-async", "defined-async", null, null)]
    [InlineData("test-async-params-empty", "empty-async", null, false)]
    public async Task TestAsync(string input, string? expectedSomeData, string? expectedArgsValue, bool? expectedArgsFlag) {
        await fixture.Parser.TryParseAsync(input);
        
        Assert.Equal(expectedSomeData, fixture.DataOutput.SomeData);
        Assert.Equal(expectedArgsValue, fixture.DataOutput.ArgsValue);
        Assert.Equal(expectedArgsFlag, fixture.DataOutput.ArgsFlag);
    }
}