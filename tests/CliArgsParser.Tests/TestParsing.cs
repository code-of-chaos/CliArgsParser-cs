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
    [InlineData("test-sync-params-empty --value=alpha", "empty", "alpha", false)]
    [InlineData("test-sync-params-empty --flag", "empty", null, true)]
    [InlineData("test-sync-params-empty -v=alpha", "empty", "alpha", false)]
    [InlineData("test-sync-params-empty -f", "empty", null, true)]
    [InlineData("""test-sync-params-empty --value="alpha beta" """, "empty", "alpha beta", false)]
    [InlineData("""test-sync-params-empty -v="alpha beta" """, "empty", "alpha beta", false)]
    [InlineData("""test-sync-params-empty -v="alpha beta" -f """, "empty", "alpha beta", true)]
    [InlineData("""test-sync-params-empty --value="alpha beta" --flag """, "empty", "alpha beta", true)]
    [InlineData("""test-sync-params-empty -v="alpha" -f """, "empty", "alpha", true)]
    [InlineData("""test-sync-params-empty --value="alpha" --flag """, "empty", "alpha", true)]
    [InlineData("test-sync-params-empty -v=alpha -f ", "empty", "alpha", true)]
    [InlineData("test-sync-params-empty --value=alpha --flag ", "empty", "alpha", true)]
    [InlineData("test-sync-params-empty --value=alpha-beta --flag ", "empty", "alpha-beta", true)]
    public void TestSync(string input, string? expectedSomeData, string? expectedArgsValue, bool? expectedArgsFlag) {

        fixture.Parser.TryParse(input);

        Assert.Equal(expectedSomeData, fixture.DataOutput.SomeData);
        Assert.Equal(expectedArgsValue, fixture.DataOutput.ArgsValue);
        Assert.Equal(expectedArgsFlag, fixture.DataOutput.ArgsFlag);
    }

    [Theory]
    [InlineData("test-async", "defined-async", null, null)]
    [InlineData("test-async-params-empty", "empty-async", null, false)]
    [InlineData("test-async-params-empty --value=alpha", "empty-async", "alpha", false)]
    [InlineData("test-async-params-empty --flag", "empty-async", null, true)]
    [InlineData("test-async-params-empty -v=alpha", "empty-async", "alpha", false)]
    [InlineData("test-async-params-empty -f", "empty-async", null, true)]
    [InlineData("""test-async-params-empty --value="alpha beta" """, "empty-async", "alpha beta", false)]
    [InlineData("""test-async-params-empty -v="alpha beta" """, "empty-async", "alpha beta", false)]
    [InlineData("""test-async-params-empty --value="alpha" --flag """, "empty-async", "alpha", true)]
    [InlineData("test-async-params-empty -v=alpha -f ", "empty-async", "alpha", true)]
    [InlineData("test-async-params-empty --value=alpha --flag ", "empty-async", "alpha", true)]
    [InlineData("test-async-params-empty --value=alpha-beta --flag ", "empty-async", "alpha-beta", true)]
    public async Task TestAsync(string input, string? expectedSomeData, string? expectedArgsValue, bool? expectedArgsFlag) {
        await fixture.Parser.TryParseAsync(input);

        Assert.Equal(expectedSomeData, fixture.DataOutput.SomeData);
        Assert.Equal(expectedArgsValue, fixture.DataOutput.ArgsValue);
        Assert.Equal(expectedArgsFlag, fixture.DataOutput.ArgsFlag);
    }

    [Theory]
    [InlineData("test-sync-params-empty --value=alpha --flag && test-sync-other", "empty", "something", "alpha", true)]
    public void TestSyncMultiple(string input, string? expectedSomeData, string? expectedSomeOtherData, string? expectedArgsValue, bool? expectedArgsFlag) {

        fixture.Parser.TryParse(input);

        Assert.Equal(expectedSomeData, fixture.DataOutput.SomeData);
        Assert.Equal(expectedSomeOtherData, fixture.DataOutput.SomeOtherData);
        Assert.Equal(expectedArgsValue, fixture.DataOutput.ArgsValue);
        Assert.Equal(expectedArgsFlag, fixture.DataOutput.ArgsFlag);
    }

    [Theory]
    [InlineData("test-async-params-empty --value=alpha --flag && test-async-other", "empty-async", "something-async", "alpha", true)]
    public async Task TestASyncMultiple(string input, string? expectedSomeData, string? expectedSomeOtherData, string? expectedArgsValue, bool? expectedArgsFlag) {

        await fixture.Parser.TryParseAsync(input);

        Assert.Equal(expectedSomeData, fixture.DataOutput.SomeData);
        Assert.Equal(expectedSomeOtherData, fixture.DataOutput.SomeOtherData);
        Assert.Equal(expectedArgsValue, fixture.DataOutput.ArgsValue);
        Assert.Equal(expectedArgsFlag, fixture.DataOutput.ArgsFlag);
    }
}
