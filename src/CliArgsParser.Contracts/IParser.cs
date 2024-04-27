// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace CliArgsParser.Contracts;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IParser {
    
    public bool TryParse(string input);
    public Task<bool> TryParseAsync(string input);

    public bool TryParse<T>(string input, out T? output);
    public Task<bool> TryParseAsync<T>(string input, out T? output);
}