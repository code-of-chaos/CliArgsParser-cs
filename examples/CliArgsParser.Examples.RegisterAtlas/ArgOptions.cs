﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Attributes;

namespace CliArgsParser.Examples.RegisterAtlas;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ArgOptions : ParameterOptions {
    [ArgValue('u', "username")]  public string? Username { get; set; }
}