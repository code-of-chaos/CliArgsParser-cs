// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using CliArgsParser.Tests.Data.Params;

namespace CliArgsParser.Tests.Data.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[CommandAtlas]
public class CommandAtlas {
    [Command("test-sync")]
    public void CallbackTestSync() {
        throw new SuccessException();
    }

    [Command("test-async")]
    public async Task CallbackTestAsync() {
        await Task.Delay(1);
        throw new SuccessException();
    }


    [Command<TestArgs>("test-sync-params-empty")]
    public void CallbackTestSyncParamsEmpty(TestArgs args) {
        Assert.True(args.Value == null);
        Assert.True(args.Flag == false);
        throw new SuccessException();
    }

    [Command<TestArgs>("test-async-params-empty")]
    public async Task CallbackTestAsyncParamsEmpty(TestArgs args) {
        await Task.Delay(1);
        Assert.True(args.Value == null);
        Assert.True(args.Flag == false);
        throw new SuccessException();
    }
    
}
