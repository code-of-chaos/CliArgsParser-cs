// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Tests.Data.Fixture;

namespace CliArgsParser.Tests;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TestHeadlessParsing(ArgsParserHeadlessFixture fixture) : IClassFixture<ArgsParserHeadlessFixture> {
    [Theory]
    [InlineData("", "empty", null, false)]
    [InlineData("--value=alpha", "empty", "alpha", false)]
    [InlineData("--flag", "empty", null, true)]
    [InlineData("-v=alpha", "empty", "alpha", false)]
    [InlineData("-f", "empty", null, true)]
    [InlineData("""--value="alpha beta" """, "empty", "alpha beta", false)]
    [InlineData("""-v="alpha beta" """, "empty", "alpha beta", false)]
    [InlineData("""-v="alpha beta" -f """, "empty", "alpha beta", true)]
    [InlineData("""--value="alpha beta" --flag """, "empty", "alpha beta", true)]
    [InlineData("""-v="alpha" -f """, "empty", "alpha", true)]
    [InlineData("""--value="alpha" --flag """, "empty", "alpha", true)]
    [InlineData("-v=alpha -f ", "empty", "alpha", true)]
    [InlineData("--value=alpha --flag ", "empty", "alpha", true)]
    [InlineData("--value=alpha-beta --flag ", "empty", "alpha-beta", true)]
    public void TestSync(string input, string? expectedSomeData, string? expectedArgsValue, bool? expectedArgsFlag) {

        fixture.Parser.Parse(input);

        Assert.Equal(expectedSomeData, fixture.DataOutput.SomeData);
        Assert.Equal(expectedArgsValue, fixture.DataOutput.ArgsValue);
        Assert.Equal(expectedArgsFlag, fixture.DataOutput.ArgsFlag);
    }
}
