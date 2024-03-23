// BitmapParser

using System.Drawing;

namespace BitmapParser;

/// <summary>
/// Represents a provider for bitmap images.
/// </summary>
public class BitmapRepository : IBitmapRepository {
	/// <summary>
	/// Gets a value indicating whether the state of the bitmap provider is valid.
	/// </summary>
	/// <remarks>
	/// The state of the bitmap provider is considered valid if the length of the original bitmap array is greater than zero.
	/// </remarks>
	public bool IsStateValid {
		get {
			if (m_original == null)
				throw new NullReferenceException(EXCEPTION_NULL_ORIGINAL_ARRAY);
			return m_original.Length > 0;
		}
	}

	/// <summary>
	/// Gets the number of bitmap images in the repository.
	/// </summary>
	/// <returns>The number of bitmap images.</returns>
	public int Count {
		get {
			if (m_original == null)
				throw new NullReferenceException(EXCEPTION_NULL_ORIGINAL_ARRAY);
			return m_original.Length;
		}
	}

	// /// <summary>
	// /// Represents a parser that can parse and manipulate bitmap images.
	// /// </summary>
	// /// <remarks>
	// /// The <see cref="BitmapParser"/> class provides methods for retrieving, modifying, and saving bitmap images.
	// /// </remarks>
	// public string[] Paths {
	// 	get {
	// 		if (m_paths == null || m_paths.Length < 1)
	// 			throw new NullReferenceException(EXCEPTION_NULL_PATHS_ARRAY);
	// 		return m_paths;
	// 	}
	// }

	/// <summary>
	/// Represents a parser that can parse and manipulate bitmap images.
	/// </summary>
	/// <remarks>
	/// The <see cref="BitmapParser"/> class provides methods for retrieving, modifying, and saving bitmap images.
	/// </remarks>
	public string[] Paths {
		get {
			if (m_paths == null || m_paths.Length < 1)
				throw new NullReferenceException(EXCEPTION_NULL_PATHS_ARRAY);
			return m_paths;
		}
	}

	// /// <summary>
	// /// Gets or sets the modified bitmaps.
	// /// </summary>
	// /// <remarks>
	// /// The modified bitmaps are the result of parsing and manipulating the original bitmaps.
	// /// </remarks>
	// public Bitmap[] Modified {
	// 	get {
	// 		if (m_modified == null || m_modified.Length < 1)
	// 			throw new NullReferenceException(EXCEPTION_NULL_MODIFIED_ARRAY);
	// 		return m_modified;
	// 	}
	// }

	/// <summary>
	/// Represents a reference to the modified bitmap images provided by a <see cref="BitmapRepository"/>.
	/// </summary>
	/// <remarks>
	/// The reference to the modified bitmap images allows for direct modification of the underlying bitmap array.
	/// </remarks>
	/// <seealso cref="BitmapRepository"/>
	/// <seealso cref="BitmapRepository.Modified"/>
	public Bitmap[] Modified {
		get {
			if (m_modified == null || m_modified.Length < 1)
				throw new NullReferenceException(EXCEPTION_NULL_MODIFIED_ARRAY);
			return m_modified;
		}
	}

	// /// BitmapParser class is responsible for parsing and manipulating bitmap images.
	// /// /
	// public Bitmap[] Original {
	// 	get {
	// 		if (m_original == null || m_original.Length < 1)
	// 			throw new NullReferenceException(EXCEPTION_NULL_ORIGINAL_ARRAY);
	// 		return m_original;
	// 	}
	// }

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
	public Bitmap[] Original {
		get {
			if (m_original == null || m_original.Length < 1)
				throw new NullReferenceException(EXCEPTION_NULL_ORIGINAL_ARRAY);
			return m_original;
		}
	}

	// /// <summary>
	// /// Retrieves a specific Bitmap object from the BitmapParser's array, using a provided index.
	// /// </summary>
	// /// <param name="bitmapIndex">The index of the Bitmap object to be retrieved.</param>
	// /// <returns>A reference to the Bitmap object at the specified index.</returns>
	// /// <exception cref="NullReferenceException">Thrown when the internal Bitmap array has not been initialized.</exception>
	// /// <exception cref="IndexOutOfRangeException">Thrown when the provided index is outside the bounds of the Bitmap array.</exception>
	// public ref Bitmap GetOriginal(int bitmapIndex) {
	// 	if (m_original == null)
	// 		throw new NullReferenceException(EXCEPTION_NULL_ORIGINAL_ARRAY);
	//
	// 	if (bitmapIndex > m_original.Length - 1 || bitmapIndex < 0)
	// 		throw new IndexOutOfRangeException(EXCEPTION_INDEX_OUT_OF_RANGE);
	//
	// 	return m_original[bitmapIndex];
	// }

	/// <summary>
	/// Retrieves a specific Bitmap object from the BitmapParser's array, using a provided index.
	/// </summary>
	/// <param name="bitmapIndex">The index of the Bitmap object to be retrieved.</param>
	/// <returns>A reference to the Bitmap object at the specified index.</returns>
	/// <exception cref="NullReferenceException">Thrown when the internal Bitmap array has not been initialized.</exception>
	/// <exception cref="IndexOutOfRangeException">Thrown when the provided index is outside the bounds of the Bitmap array.</exception>
	public Bitmap GetOriginal(int bitmapIndex) {
		if (m_original == null)
			throw new NullReferenceException(EXCEPTION_NULL_ORIGINAL_ARRAY);

		if (bitmapIndex > m_original.Length - 1 || bitmapIndex < 0)
			throw new IndexOutOfRangeException(EXCEPTION_INDEX_OUT_OF_RANGE);

		return  m_original[bitmapIndex];
	}

	/// <summary>
	/// Retrieves a specific Bitmap object from the BitmapParser's modified array, using a provided index.
	/// </summary>
	/// <param name="bitmapIndex">The index of the Bitmap object to be retrieved.</param>
	/// <returns>A reference to the Bitmap object at the specified index.</returns>
	/// <exception cref="NullReferenceException">Thrown when the internal Bitmap array has not been initialized.</exception>
	/// <exception cref="IndexOutOfRangeException">Thrown when the provided index is outside the bounds of the Bitmap array.</exception>
	public  Bitmap GetModified(int bitmapIndex) {
		if (m_modified == null)
			throw new NullReferenceException(EXCEPTION_NULL_MODIFIED_ARRAY);

		if (bitmapIndex > m_modified.Length - 1 || bitmapIndex < 0)
			throw new IndexOutOfRangeException(EXCEPTION_INDEX_OUT_OF_RANGE);

		return  m_modified[bitmapIndex];
	}

	/// <summary>
	/// Swaps a specified Bitmap object at the given index with a new Bitmap object.
	/// </summary>
	/// <param name="index">The index of the Bitmap object to be swapped.</param>
	/// <param name="bmp">The new Bitmap object to be swapped.</param>
	/// <exception cref="NullReferenceException">Thrown when the internal Bitmap array has not been initialized.</exception>
	/// <exception cref="IndexOutOfRangeException">Thrown when the provided index is outside the bounds of the Bitmap array.</exception>
	public void Swap(int index, Bitmap bmp) {
		if (m_modified == null)
			throw new NullReferenceException(EXCEPTION_NULL_ORIGINAL_ARRAY);

		if (index > m_original.Length - 1 || index < 0)
			throw new IndexOutOfRangeException(EXCEPTION_INDEX_OUT_OF_RANGE);

		m_modified[index].Dispose();
		m_modified[index] = bmp;
	}

	// /// <summary>
	// /// Retrieves the file path associated with a specific bitmap index.
	// /// </summary>
	// /// <param name="bitmapIndex">The index of the bitmap to retrieve the file path for.</param>
	// /// <returns>The file path associated with the specified bitmap index.</returns>
	// /// <exception cref="NullReferenceException">Thrown when the internal paths array has not been initialized.</exception>
	// /// <exception cref="IndexOutOfRangeException">Thrown when the provided index is outside the bounds of the paths array.</exception>
	// public string GetPath(int bitmapIndex) {
	// 	if (m_paths == null || m_paths.Length < 1)
	// 		throw new NullReferenceException(EXCEPTION_NULL_PATHS_ARRAY);
	//
	// 	if (bitmapIndex > m_paths.Length || bitmapIndex < 0)
	// 		throw new IndexOutOfRangeException(EXCEPTION_INDEX_OUT_OF_RANGE);
	//
	// 	return m_paths[bitmapIndex];
	// }

	/// <summary>
	/// Represents a repository for bitmap images.
	/// </summary>
	public string GetPath(int bitmapIndex) {
		if (bitmapIndex > m_paths.Length || bitmapIndex < 0)
			throw new IndexOutOfRangeException(EXCEPTION_INDEX_OUT_OF_RANGE);

		if (m_paths == null)
			throw new NullReferenceException(EXCEPTION_NULL_PATHS_ARRAY);
		
		return m_paths[bitmapIndex];
	}

	bool IsDisposed { get; set; }

#region PLUMBING

	public BitmapRepository(string[] paths) {
		m_paths    = paths;
		m_modified = new Bitmap[paths.Length];
		m_original = new Bitmap[paths.Length];

		for (var i = 0; i < paths.Length; i++) {
			if (!File.Exists(paths[i]))
				throw new FileNotFoundException(EXCEPTION_FILE_NOT_FOUND + " " + paths[i]);

			var bmp = new Bitmap(paths[i]);
			m_original[i] = bmp;
		}
	}

	readonly Bitmap[] m_modified;
	readonly Bitmap[] m_original;
	readonly string[] m_paths;

#endregion

#region EXCEPTION_CONSTANTS

	internal const string EXCEPTION_FILE_NOT_FOUND      = "Could not find the file name ";
	internal const string EXCEPTION_NULL_MODIFIED_ARRAY = "Modified bitmap array is null.";
	internal const string EXCEPTION_NULL_ORIGINAL_ARRAY = "Original bitmap array is null.";
	internal const string EXCEPTION_NULL_PATHS_ARRAY    = "No paths were provided to load bitmaps from.";
	internal const string EXCEPTION_INDEX_OUT_OF_RANGE  = "Index is out of range.";

#endregion

	public void Dispose() {
		if (IsDisposed)
			return;

		for (var i = 0; i < m_original.Length; i++) {
			m_original[i].Dispose();
			m_modified[i].Dispose();
		}

		IsDisposed = true;
	}
}