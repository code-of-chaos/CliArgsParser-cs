// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Contracts.Attributes;

namespace CliArgsParser.Contracts;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents the interface for a parameter parser.
/// </summary>
/// <typeparam name="T">The type of the parameter options.</typeparam>
public interface IParameterParser<out T> where T: IParameterOptions, new() {
    /// <summary>
    /// Returns a collection of readable descriptions for the arguments defined in the parameter options.
    /// Each description includes the short name, long name, and description of the argument.
    /// If the description is not provided, "UNKNOWN DESCRIPTION" is used.
    /// </summary>
    /// <returns>A collection of readable descriptions for the arguments.</returns>
    public IEnumerable<string> GetDescriptionsReadable();

    /// <summary>
    /// Retrieves the descriptions of the command line arguments in a readable format.
    /// </summary>
    /// <typeparam name="TT">The attribute type to filter the descriptions.</typeparam>
    /// <returns>
    /// An IEnumerable of type TT containing the descriptions of the command line arguments
    /// in a readable format.
    /// </returns>
    public IEnumerable<TT?> GetDescriptions<TT>() where TT : Attribute, IArgAttribute;

    /// <summary>
    /// Parses the command-line arguments and returns an instance of the specified parameter options type.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    /// <returns>An instance of the specified parameter options type.</returns>
    public T Parse(IEnumerable<string> args);
}