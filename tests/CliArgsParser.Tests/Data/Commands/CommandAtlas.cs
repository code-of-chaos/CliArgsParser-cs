// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using CliArgsParser.Tests.Data.Fixture;
using CliArgsParser.Tests.Data.Params;

namespace CliArgsParser.Tests.Data.Commands;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[CommandAtlas]
public class CommandAtlas(DataOutput data) {
    [Command("test-sync")]
    public void CallbackTestSync() {
        data.SomeData = "defined";
        data.ArgsFlag = null;
        data.ArgsValue = null;
    }

    [Command("test-async")]
    public async Task CallbackTestAsync() {
        await Task.Delay(1);
        data.SomeData = "defined-async";
        data.ArgsFlag = null;
        data.ArgsValue = null;
    }


    [Command<TestArgs>("test-sync-params-empty")]
    public void CallbackTestSyncParamsEmpty(TestArgs args) {
        data.SomeData = "empty";
        data.ArgsValue = args.Value;
        data.ArgsFlag = args.Flag;
    }

    [Command<TestArgs>("test-async-params-empty")]
    public async Task CallbackTestAsyncParamsEmpty(TestArgs args) {
        await Task.Delay(1);
        data.SomeData = "empty-async";
        data.ArgsValue = args.Value;
        data.ArgsFlag = args.Flag;
    }

    [Command("test-sync-other")]
    public void CallbackTestOtherSync() {
        data.SomeOtherData = "something";
    }

    [Command("test-async-other")]
    public async Task CallbackTestOtherAsync() {
        await Task.Delay(1);
        data.SomeOtherData = "something-async";
    }
}
