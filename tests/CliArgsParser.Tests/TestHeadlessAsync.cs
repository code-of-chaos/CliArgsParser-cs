// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Tests.Data.Fixture;

namespace CliArgsParser.Tests;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TestHeadlessAsyncParsing(ArgsParserHeadlessAsyncFixture fixture) : IClassFixture<ArgsParserHeadlessAsyncFixture> {
    [Theory]
    [InlineData("", "empty", null, false)]
    [InlineData("--value=alpha", "empty", "alpha", false)]
    [InlineData("--flag", "empty", null, true)]
    [InlineData("-v=alpha", "empty", "alpha", false)]
    [InlineData("-f", "empty", null, true)]
    [InlineData("""--value="alpha beta" """, "empty", "alpha beta", false)]
    [InlineData("""-v="alpha beta" """, "empty", "alpha beta", false)]
    [InlineData("""--value="alpha" --flag """, "empty", "alpha", true)]
    [InlineData("-v=alpha -f ", "empty", "alpha", true)]
    [InlineData("--value=alpha --flag ", "empty", "alpha", true)]
    [InlineData("--value=alpha-beta --flag ", "empty", "alpha-beta", true)]
    public async Task TestAsync(string input, string? expectedSomeData, string? expectedArgsValue, bool? expectedArgsFlag) {
        await fixture.Parser.ParseAsyncLinear(input);

        Assert.Equal(expectedSomeData, fixture.DataOutput.SomeData);
        Assert.Equal(expectedArgsValue, fixture.DataOutput.ArgsValue);
        Assert.Equal(expectedArgsFlag, fixture.DataOutput.ArgsFlag);
    }
}
