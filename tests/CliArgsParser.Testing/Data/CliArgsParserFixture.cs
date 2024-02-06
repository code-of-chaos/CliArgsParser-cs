﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CliArgsParser.Testing.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CliArgsParserFixture : IDisposable {
    public ICliArgsParser Parser = new CliArgsParser().RegisterFromCliAtlas(new CommandAtlas());

    public void Dispose() {
        
    }
}