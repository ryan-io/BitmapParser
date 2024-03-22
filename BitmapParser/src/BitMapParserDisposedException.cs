// BitmapParser

namespace BitmapParser;

/// <summary>
/// The exception that is thrown when an operation is performed on a disposed instance of BitmapParser.
/// </summary>
public class BitMapParserDisposedException : Exception {
	/// <summary>
	/// Gets the message that describes the exception.
	/// </summary>
	/// <value>
	/// A string that describes the exception.
	/// </value>
	public override string Message => EXCEPTION_BITMAP_PARSER_DISPOSED;

	internal const string EXCEPTION_BITMAP_PARSER_DISPOSED =
		"Instance of BitmapParser has been disposed. No further actions can be taken. Please create a new parser.";
}