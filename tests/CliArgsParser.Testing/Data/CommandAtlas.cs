// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CliArgsParser.Testing.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class CommandAtlas : CliCommandAtlas {
    [CliCommand<NoArgs>("always-true")]
    public bool CallbackAlwaysTrue(NoArgs _) {
        return true;
    }

    [CliCommand<NoArgs>("always-false")]
    public bool CallbackVoid() {
        return false;
    }

    [CliCommand<TestArgs>("test-empty")]
    public void CallbackTestEmpty(TestArgs testArgs) {
        Assert.Null(testArgs.Value);
        Assert.False(testArgs.Flag);
    }

    [CliCommand<TestArgs>("test-data")]
    public void CallbackTestData(TestArgs testArgs) {
        Assert.True(testArgs.Flag);
        Assert.Equal("data", testArgs.Value);
    }

    [CliCommand<TestArgs>("test-data-verbose")]
    public void CallbackTestDataVerbose(TestArgs testArgs) {
        Assert.True(testArgs.Verbose);
        Assert.True(testArgs.Flag);
        Assert.Equal("data", testArgs.Value);
    }
}
