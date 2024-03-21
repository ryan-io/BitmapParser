// BitmapParser

using System.Drawing;

namespace BitmapParser;

/// <summary>
/// Represents a provider for bitmap images.
/// </summary>
public interface IBitmapRepository : IDisposable {
	/// <summary>
	/// Represents a reference to the original bitmap images provided by the BitmapProvider.
	/// </summary>
	/// <remarks>
	/// The OriginalRef property is a reference to the original bitmap images provided by the BitmapProvider.
	/// </remarks>
	/// <example>
	/// <code>
	/// // Get the reference to the original bitmap images
	/// ref Bitmap[] original = bitmapProvider.OriginalRef;
	/// </code>
	/// </example>
	Bitmap[] Original { get; }

	/// <summary>
	/// Represents a reference to the modified bitmap images provided by a <see cref="BitmapRepository"/>.
	/// </summary>
	/// <remarks>
	/// The reference to the modified bitmap images allows for direct modification of the underlying bitmap array.
	/// </remarks>
	/// <seealso cref="BitmapRepository"/>
	/// <seealso cref="BitmapRepository.Modified"/>
	 Bitmap[] Modified { get; }

	/// <summary>
	/// Gets a value indicating whether the state of the bitmap provider is valid.
	/// </summary>
	/// <remarks>
	/// The state of the bitmap provider is considered valid if the length of the original bitmap array is greater than zero.
	/// </remarks>
	bool IsStateValid { get; }

	// /// <summary>
	// /// Represents a parser that can parse and manipulate bitmap images.
	// /// </summary>
	// /// <remarks>
	// /// The <see cref="BitmapParser"/> class provides methods for retrieving, modifying, and saving bitmap images.
	// /// </remarks>
	// string[] Paths { get; }
	//
	// /// <summary>
	// /// Gets or sets the modified bitmaps.
	// /// </summary>
	// /// <remarks>
	// /// The modified bitmaps are the result of parsing and manipulating the original bitmaps.
	// /// </remarks>
	// Bitmap[] Modified { get; }
	//
	// /// BitmapParser class is responsible for parsing and manipulating bitmap images.
	// /// /
	// Bitmap[] Original { get; }

	/// <summary>
	/// Represents a parser that can parse and manipulate bitmap images.
	/// </summary>
	/// <remarks>
	/// The <see cref="BitmapParser"/> class provides methods for retrieving, modifying, and saving bitmap images.
	/// </remarks>
	string[] Paths { get; }

	/// <summary>
	/// Gets the number of bitmap images in the repository.
	/// </summary>
	/// <returns>The number of bitmap images.</returns>
	int Count { get; }

	/// <summary>
	/// Retrieves a specific Bitmap object from the BitmapParser's array, using a provided index.
	/// </summary>
	/// <param name="bitmapIndex">The index of the Bitmap object to be retrieved.</param>
	/// <returns>A reference to the Bitmap object at the specified index.</returns>
	/// <exception cref="NullReferenceException">Thrown when the internal Bitmap array has not been initialized.</exception>
	/// <exception cref="IndexOutOfRangeException">Thrown when the provided index is outside the bounds of the Bitmap array.</exception>
	Bitmap GetOriginal(int bitmapIndex);
	
	/// <summary>
	/// Retrieves a specific Bitmap object from the BitmapParser's modified array, using a provided index.
	/// </summary>
	/// <param name="bitmapIndex">The index of the Bitmap object to be retrieved.</param>
	/// <returns>A reference to the Bitmap object at the specified index.</returns>
	/// <exception cref="NullReferenceException">Thrown when the internal Bitmap array has not been initialized.</exception>
	/// <exception cref="IndexOutOfRangeException">Thrown when the provided index is outside the bounds of the Bitmap array.</exception>
	Bitmap GetModified(int bitmapIndex);

	// /// <summary>
	// /// Retrieves a specific Bitmap object from the BitmapParser's array, using a provided index.
	// /// </summary>
	// /// <param name="bitmapIndex">The index of the Bitmap object to be retrieved.</param>
	// /// <returns>A reference to the Bitmap object at the specified index.</returns>
	// /// <exception cref="NullReferenceException">Thrown when the internal Bitmap array has not been initialized.</exception>
	// /// <exception cref="IndexOutOfRangeException">Thrown when the provided index is outside the bounds of the Bitmap array.</exception>
	// Bitmap GetOriginal(int bitmapIndex);

	/// <summary>
	/// Swaps a specified Bitmap object at the given index with a new Bitmap object.
	/// </summary>
	/// <param name="index">The index of the Bitmap object to be swapped.</param>
	/// <param name="bmp">The new Bitmap object to be swapped.</param>
	/// <exception cref="NullReferenceException">Thrown when the internal Bitmap array has not been initialized.</exception>
	/// <exception cref="IndexOutOfRangeException">Thrown when the provided index is outside the bounds of the Bitmap array.</exception>
	void Swap(int index, Bitmap bmp);

	// /// <summary>
	// /// Retrieves the file path associated with a specific bitmap index.
	// /// </summary>
	// /// <param name="bitmapIndex">The index of the bitmap to retrieve the file path for.</param>
	// /// <returns>The file path associated with the specified bitmap index.</returns>
	// /// <exception cref="NullReferenceException">Thrown when the internal paths array has not been initialized.</exception>
	// /// <exception cref="IndexOutOfRangeException">Thrown when the provided index is outside the bounds of the paths array.</exception>
	// string GetPath(int bitmapIndex);

	/// <summary>
	/// Represents a repository for bitmap images.
	/// </summary>
	string GetPath(int bitmapIndex);
}