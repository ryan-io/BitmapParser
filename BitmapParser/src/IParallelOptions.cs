// BitmapParser

namespace BitmapParser;

/// <summary>
/// Represents the options for parallel processing in the BitmapParser class.
/// </summary>
public interface IParallelOptions {
	ParallelOptions Get { get; }
}