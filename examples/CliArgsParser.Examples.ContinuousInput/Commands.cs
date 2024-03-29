﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using CliArgsParser.Attributes;
using CliArgsParser.Commands;

namespace CliArgsParser.Examples.ContinuousInput;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class Commands : CliCommandAtlas {
    
    [CliCommand("test")]
    public bool CallbackTest(NoArgs _) {
        Console.WriteLine("Testing this");
        return false;
    }
    
    [CliCommand<NoArgs>("void")]
    public void CallbackVoid() {
        Console.WriteLine("Testing this");
    }
}