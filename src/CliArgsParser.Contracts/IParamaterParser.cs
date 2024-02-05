// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Contracts.Attributes;

namespace CliArgsParser.Contracts;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IParamaterParser<out T> where T: IParameterOptions, new() {
    public IEnumerable<string> GetDescriptionsReadable();
    public IEnumerable<TT?> GetDescriptions<TT>() where TT : Attribute, IArgAttribute;
    public T Parse(IEnumerable<string> args);
}