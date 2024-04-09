// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace CliArgsParser.Testing.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class CommandAtlasAsync : CliCommandAtlas {
    [CliCommand("test-clicommand-async-true")]
    public async Task<bool> CallbackTestCliCommandAsyncVoid() {
        await Task.Delay(1);
        return true;
    }
    [CliCommand("test-clicommand-async-false")]
    public async Task<bool> CallbackTestCliCommandAsyncVoidNeg() {
        await Task.Delay(1);
        return false;
    }
    
    [CliCommand("test-clicommand-async-spore")]
    public async Task<string> CallbackTestCliCommandAsyncSpore() {
        using var client = new HttpClient();
        var response = await client.GetAsync("https://www.spore.com/rest/stats");
        
        if (!response.IsSuccessStatusCode) {
            throw new Exception($"Failed to get data from server: {response.StatusCode}");
        }
        
        var content = await response.Content.ReadAsStringAsync();
        var x =  XDocument.Parse(content);
        return x.Root?.Name.ToString();
    }
}
