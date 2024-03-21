// BitmapParser

namespace BitmapParser;

/// <summary>
/// Represents an interface for providing image paths.
/// </summary>
public interface IImagePathProvider {
	/// <summary>
	/// Represents a parser for bitmap images.
	/// </summary>
	string[] Paths { get; }
}