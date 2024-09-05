// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable CheckNamespace
namespace CliArgsParser;
// ReSharper restore CheckNamespace

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Defines the different modes of headless operation in the CLI parser.
/// </summary>
public enum HeadlessTypes {
    /// <summary>
    /// Indicates that headless mode is disabled in the HeadlessTypes enum.
    /// </summary>
    Disabled = 0,

    /// <summary>
    /// Represents the mode where headless operation is permissible for arguments within the HeadlessTypes enum.
    /// </summary>
    AllowInputArguments = 1,

    /// <summary>
    /// Indicates that no specific requirements are necessary for headless mode.
    /// </summary>
    IgnoreInputArguments = 2,
}
