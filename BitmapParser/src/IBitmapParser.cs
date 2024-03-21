// // BitmapParser
//
// using System.Drawing;
// using System.Drawing.Imaging;
//
// namespace BitmapParser;
//
// public interface IBitmapParser : IDisposable {
// 	/// <summary>
// 	/// Returns a reference to the internal array of Bitmap objects.
// 	/// </summary>
// 	ref Bitmap[] GetAllBitmapsRef();
//
// 	/// <summary>
// 	/// Retrieves a specific Bitmap object from the BitmapParser's array, using a provided index.
// 	/// </summary>
// 	/// <param name="bitmapIndex">The index of the Bitmap object to be retrieved.</param>
// 	/// <returns>A reference to the Bitmap object at the specified index.</returns>
// 	/// <exception cref="NullReferenceException">Thrown when the internal Bitmap array has not been initialized.</exception>
// 	/// <exception cref="IndexOutOfRangeException">Thrown when the provided index is outside the bounds of the Bitmap array.</exception>
// 	ref Bitmap GetBitmapRef(int bitmapIndex);
//
// 	/// <summary>
// 	/// Returns a read-only reference to the array of the paths to all images.
// 	/// </summary>
// 	ref readonly string[] GetAllPathsRef();
//
// 	/// <summary>
// 	/// Scales the specified bitmap at the given index and replaces it with the new scaled bitmap.
// 	/// </summary>
// 	/// <param name="bitmapIndex">The index of the bitmap to be scaled.</param>
// 	/// <param name="scale">The scale factor to be applied to the bitmap.</param>
// 	/// <param name="criteria">The criteria to be used for resizing the bitmap.</param>
// 	/// <remarks>
// 	/// This method retrieves the specified bitmap at the given index, scales it according to the provided scale factor and criteria,
// 	/// disposes the original bitmap, and replaces it with the new scaled bitmap.
// 	/// </remarks>
// 	void ScaleBitmapAndSetNew(int bitmapIndex, float scale, BitmapResizeCriteria criteria);
//
// 	/// <summary>
// 	/// Sets a new bitmap at the specified index in the array of bitmaps.
// 	/// If there is an existing bitmap at the specified index, it is disposed before setting the new bitmap.
// 	/// </summary>
// 	/// <param name="bitmapIndex">The index at which to set the new bitmap.</param>
// 	/// <param name="newBmp">The new bitmap to set at the specified index.</param>
// 	void SetNewBitmap(int bitmapIndex, Bitmap newBmp);
//
// 	/// <summary>
// 	/// Returns a new scaled Bitmap based on the specified bitmap index, scale and criteria.
// 	/// </summary>
// 	/// <param name="bitmapIndex">The index of the bitmap.</param>
// 	/// <param name="scale">The scale factor.</param>
// 	/// <param name="criteria">The criteria for resizing.</param>
// 	/// <returns>A new scaled Bitmap.</returns>
// 	/// <exception cref="IndexOutOfRangeException">Thrown if the bitmap index is less than zero.</exception>
// 	/// Reference -> //https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp
// 	Bitmap GetNewScaledBitmap(int bitmapIndex, float scale, BitmapResizeCriteria criteria);
//
// 	/// <summary>
// 	/// Retrieves the path of a specific bitmap image.
// 	/// </summary>
// 	/// <param name="index">Index position of the image path in the list of paths.</param>
// 	/// <returns>Returns a reference to the path string of the image.</returns>
// 	/// <exception cref="NullReferenceException">Thrown when the list of paths is null.</exception>
// 	/// <exception cref="IndexOutOfRangeException">Thrown when index is out of the range of the list of paths.</exception>
// 	ref string GetPathRef(int index);
//
// 	/// <summary>
// 	/// 
// 	/// Modifies the RGB values of a Bitmap at a specific index in an unsafe and concurrent manner. 
// 	/// This code is base on: https://csharpexamples.com/fast-image-processing-c/
// 	///		from author Turgay
// 	/// Modifies the RGB values of a Bitmap at a specific index in an unsafe and concurrent manner.
// 	/// </summary>
// 	/// <param name="bitmapIndex">The index of the Bitmap in the internal array.</param>
// 	/// <param name="functor">A delegate function that performs the desired modifications on the pixel data.</param>
// 	/// <returns>Returns reference to the internal array of Bitmap objects.</returns>
// 	ref Bitmap ModifyRgbUnsafeRef(int bitmapIndex, BitmapParser.BitmapRgbDelegate functor);
//
// 	/// <summary>
// 	/// Modifies the RGB values of multiple bitmaps using the provided functor.
// 	/// </summary>
// 	/// <param name="functor">The delegate that specifies how the RGB values should be modified.</param>
// 	/// <param name="bitmapIndices">The indices of the bitmaps to modify.</param>
// 	/// <returns>A reference to an array of modified bitmaps.</returns>
// 	ref Bitmap[] ModifyRgbUnsafeRef(BitmapParser.BitmapRgbDelegate functor, params int[] bitmapIndices);
//
// 	/// <summary>
// 	/// Modifies the RGB values of all the bitmaps in the array using the specified functor
// 	/// </summary>
// 	/// <param name="functor">The delegate that defines the modification operation</param>
// 	/// <returns>A reference to the modified array of bitmaps</returns>
// 	ref Bitmap[] ModifyAllRgbUnsafeRef(BitmapParser.BitmapRgbDelegate functor);
//
// 	/// <summary>
// 	/// Asynchronously saves Bitmap objects to the files at a specified path.
// 	/// </summary>
// 	/// <param name="path">The directory path where the Bitmaps should be saved.</param>
// 	/// <param name="disposeOnSuccess">Optional parameter determining whether to dispose the BitmapParser on successful save. Default is false.</param>
// 	/// <param name="format">Format to save each bitmap as.</param>
// 	/// <param name="token">Optional cancellation token</param>
// 	/// <param name="createPathIfNotExit">If true, will create the directory if it does not exist</param>
// 	/// <returns>Returns a Task that represents the asynchronous operation. The task result contains void.</returns>
// 	/// <exception cref="DirectoryNotFoundException">Thrown when the provided path is null, empty, or consists only of white-space characters.</exception>
// 	Task<List<string>> SaveBitmapsAsync(string path, ImageFormat format, CancellationToken token, bool
// 		disposeOnSuccess
// 		= false, bool createPathIfNotExit = false);
//
// 	/// <summary>
// 	/// Gets a value indicating whether the object has been disposed.
// 	/// </summary>
// 	/// <value>
// 	/// <c>true</c> if the object has been disposed; otherwise, <c>false</c>.
// 	/// </value>
// 	bool IsDisposed { get; }
//
// 	/// <summary>
// 	/// Returns a copy array of all Bitmap objects.
// 	/// </summary>
// 	/// <returns>An array of Bitmap objects.</returns>
// 	Bitmap[] GetAllBitmaps();
//
// 	/// <summary>
// 	/// Retrieves a specific Bitmap object from the BitmapParser's array, using a provided index.
// 	/// </summary>
// 	/// <param name="bitmapIndex">The index of the Bitmap object to be retrieved.</param>
// 	/// <returns>A reference to the Bitmap object at the specified index.</returns>
// 	/// <exception cref="NullReferenceException">Thrown when the internal Bitmap array has not been initialized.</exception>
// 	/// <exception cref="IndexOutOfRangeException">Thrown when the provided index is outside the bounds of the Bitmap array.</exception>
// 	Bitmap GetBitmap(int bitmapIndex);
//
// 	/// <summary>
// 	/// Returns a copy array of the paths to all images.
// 	/// </summary>
// 	string[] GetAllPaths();
//
// 	/// <summary>
// 	/// Retrieves the path of a specific bitmap image.
// 	/// </summary>
// 	/// <param name="index">Index position of the image path in the list of paths.</param>
// 	/// <returns>Returns a reference to the path string of the image.</returns>
// 	/// <exception cref="NullReferenceException">Thrown when the list of paths is null.</exception>
// 	/// <exception cref="IndexOutOfRangeException">Thrown when index is out of the range of the list of paths.</exception>
// 	string GetPath(int index);
//
// 	/// <summary>
// 	/// Modifies the RGB values of a Bitmap at a specific index in an unsafe and concurrent manner. 
// 	/// This code is base on: https://csharpexamples.com/fast-image-processing-c/
// 	///		from author Turgay
// 	/// Modifies the RGB values of a Bitmap at a specific index in an unsafe and concurrent manner.
// 	/// This makes a call to an internally unsafe method
// 	/// </summary>
// 	/// <param name="bitmapIndex">The index of the Bitmap in the internal array.</param>
// 	/// <param name="functor">A delegate function that performs the desired modifications on the pixel data.</param>
// 	/// <returns>Returns reference to the internal array of Bitmap objects.</returns>
// 	Bitmap ModifyRgbUnsafe(int bitmapIndex, BitmapParser.BitmapRgbDelegate functor);
//
// 	/// <summary>
// 	/// Modifies the RGB values of multiple bitmaps using the provided functor.
// 	/// </summary>
// 	/// <param name="functor">The delegate that specifies how the RGB values should be modified.</param>
// 	/// <param name="bitmapIndices">The indices of the bitmaps to modify.</param>
// 	/// <returns>A reference to an array of modified bitmaps.</returns>
// 	Bitmap[] ModifyRgbUnsafe(BitmapParser.BitmapRgbDelegate functor, params int[] bitmapIndices);
//
// 	/// <summary>
// 	/// Modifies the RGB values of all the bitmaps in the array using the specified functor
// 	/// This calls an internally unsafe method.
// 	/// </summary>
// 	/// <param name="functor">The delegate that defines the modification operation</param>
// 	/// <returns>A reference to the modified array of bitmaps</returns>
// 	Bitmap[] ModifyAllRgbUnsafe(BitmapParser.BitmapRgbDelegate functor);
// }