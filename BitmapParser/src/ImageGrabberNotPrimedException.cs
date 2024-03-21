// BitmapParser

namespace BitmapParser;

/// <summary>
/// Represents an exception that is thrown when an instance of ImageGrabber exists, but no file directory has been provided.
/// </summary>
public class ImageGrabberNotPrimedException : Exception {
	/// <summary>
	/// Gets the error message that describes the exceptional condition.
	/// </summary>
	/// <value>
	/// The error message that explains the reason for the exception.
	/// </value>
	public override string Message
		=> "An instance of ImageGrabber exists, but no file directory has been provided.";
}