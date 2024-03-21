// BitmapParser

namespace BitmapParser;

/// <summary>
/// Represents an exception that is thrown when no image parser has been assigned.
/// </summary>
public class NoImageParserAssignedException : Exception {
	/// <summary>
	/// Gets the message for when no image parser has been assigned.
	/// </summary>
	/// <value>
	/// The message for when no image parser has been assigned.
	/// </value>
	public override string Message => "No image parser has been assigned";
}