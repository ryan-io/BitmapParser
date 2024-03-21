// BitmapParser

namespace BitmapParser;

public static class ParallelOptionsDefault {
	public static ParallelOptions Get() => new ParallelOptions {
		MaxDegreeOfParallelism = Environment.ProcessorCount
	};
}

/// <summary>
/// Represents the options for parallel processing in the BitmapParser class.
/// </summary>
public interface IParallelOptions {
	ParallelOptions Get { get; }
}