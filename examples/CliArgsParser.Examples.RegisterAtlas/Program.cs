﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CliArgsParser.Examples.RegisterAtlas;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
static class Program {
    public static void Main(string[] args) {
        
        new CliArgsParser()
            .RegisterFromCliAtlas(new CommandsStatic())
            .TryParse(args, true);
        
    }
}