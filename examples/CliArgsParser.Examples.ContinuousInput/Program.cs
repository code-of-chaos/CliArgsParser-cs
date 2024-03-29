﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CliArgsParser.Examples.ContinuousInput;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
static class Program {
    public static void Main(string[] _) {
        new CliArgsParser()
            .RegisterFromCliAtlas(new Commands())
            .TryParseInput(allowMultiple:true);
    }
}