using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;

namespace BitmapParser {
	/// <summary>
	/// Parses and manipulates bitmap images.
	/// </summary>
	[System.Runtime.Versioning.SupportedOSPlatform("windows")]
	public sealed class BitmapParser : IDisposable {
		/// <summary>
		/// A delegate type for manipulating the RGB values of a particular pixel in a bitmap.
		/// </summary>
		public delegate void BitmapRgbDelegate(ref int pxlIndex, ref int red, ref int green, ref int blue);

		/// <summary>
		/// Gets the count of Bitmap objects in the BitmapParser repository.
		/// </summary>
		/// <value>The count of Bitmap objects.</value>
		public int Count => m_repository.Count;

		/// <summary>
		/// Gets a value indicating whether the object has been disposed.
		/// </summary>
		/// <value>
		/// <c>true</c> if the object has been disposed; otherwise, <c>false</c>.
		/// </value>
		public bool IsDisposed { get; private set; }

		/// <summary>
		/// Returns a reference to the internal original array of Bitmap objects.
		/// </summary>
		public Bitmap[] GetAllOriginalBitmaps() {
			ValidateInternalState();
			return m_repository.Original;
		}

		/// <summary>
		/// Returns a reference to the internal modified array of Bitmap objects.
		/// </summary>
		public Bitmap[] GetAllModifiedBitmaps() {
			ValidateInternalState();
			return m_repository.Modified;
		}

		/// <summary>
		/// Retrieves a specific Bitmap object from the BitmapParser's array, using a provided index.
		/// </summary>
		/// <param name="bitmapIndex">The index of the Bitmap object to be retrieved.</param>
		/// <returns>A reference to the Bitmap object at the specified index.</returns>
		/// <exception cref="NullReferenceException">Thrown when the internal Bitmap array has not been initialized.</exception>
		/// <exception cref="IndexOutOfRangeException">Thrown when the provided index is outside the bounds of the Bitmap array.</exception>
		public Bitmap GetOriginal(int bitmapIndex) {
			ValidateInternalState();
			return m_repository.GetOriginal(bitmapIndex);
		}

		/// <summary>
		/// Retrieves a specific Bitmap object from the BitmapParser's modified array, using a provided index.
		/// </summary>
		/// <param name="bitmapIndex">The index of the Bitmap object to be retrieved.</param>
		/// <returns>A reference to the Bitmap object at the specified index.</returns>
		/// <exception cref="NullReferenceException">Thrown when the internal Bitmap array has not been initialized.</exception>
		/// <exception cref="IndexOutOfRangeException">Thrown when the provided index is outside the bounds of the Bitmap array.</exception>
		public Bitmap GetModified(int bitmapIndex) {
			if (IsDisposed)
				throw new BitMapParserDisposedException();

			return m_repository.GetModified(bitmapIndex);
		}

		/// <summary>
		/// Returns a read-only reference to the array of the paths to all images.
		/// </summary>
		public string[] GetPaths() => m_repository.Paths;

		/// <summary>
		/// Retrieves the path of a specific bitmap image.
		/// </summary>
		/// <param name="index">Index position of the image path in the list of paths.</param>
		/// <returns>Returns a reference to the path string of the image.</returns>
		/// <exception cref="NullReferenceException">Thrown when the list of paths is null.</exception>
		/// <exception cref="IndexOutOfRangeException">Thrown when index is out of the range of the list of paths.</exception>
		public string GetPath(int index) => m_repository.GetPath(index);

		/// <summary>
		/// Scales the specified bitmap at the given index and replaces it with the new scaled bitmap.
		/// </summary>
		/// <param name="bitmapIndex">The index of the bitmap to be scaled.</param>
		/// <param name="scale">The scale factor to be applied to the bitmap.</param>
		/// <param name="criteria">The criteria to be used for resizing the bitmap.</param>
		/// <remarks>
		/// This method retrieves the specified bitmap at the given index, scales it according to the provided scale factor and criteria,
		/// disposes the original bitmap, and replaces it with the new scaled bitmap.
		/// </remarks>
		public void ScaleBitmapAndSetNew(int bitmapIndex, float scale, BitmapResizeCriteria criteria) {
			var newBmp = GetNewScaledBitmap(bitmapIndex, scale, criteria);
			m_repository.Swap(bitmapIndex, newBmp);
		}

		/// <summary>
		/// Sets a new bitmap at the specified index in the array of bitmaps.
		/// If there is an existing bitmap at the specified index, it is disposed before setting the new bitmap.
		/// </summary>
		/// <param name="bitmapIndex">The index at which to set the new bitmap.</param>
		/// <param name="newBmp">The new bitmap to set at the specified index.</param>
		public Bitmap SwapBitmaps(int bitmapIndex, Bitmap newBmp) {
			return m_repository.Swap(bitmapIndex, newBmp);
		}

		/// <summary>
		/// Returns a new scaled Bitmap based on the specified bitmap index, scale and criteria.
		/// </summary>
		/// <param name="bitmapIndex">The index of the bitmap.</param>
		/// <param name="scale">The scale factor.</param>
		/// <param name="criteria">The criteria for resizing.</param>
		/// <returns>A new scaled Bitmap.</returns>
		/// <exception cref="IndexOutOfRangeException">Thrown if the bitmap index is less than zero.</exception>
		/// Reference -> //https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp
		public Bitmap GetNewScaledBitmap(int bitmapIndex, float scale, BitmapResizeCriteria criteria) {
			var bmp = GetOriginal(bitmapIndex);
			return GetNewScaledBitmap(bmp, scale, criteria);
		}

		/// <summary>
		/// Scales the given bitmap to a new size based on the specified scale and resizing criteria.
		/// </summary>
		/// <param name="bmp">The original bitmap to be scaled.</param>
		/// <param name="scale">The scaling factor. Must be a positive value.</param>
		/// <param name="criteria">The criteria used for resizing. Determines the quality of the resized image.</param>
		/// <returns>
		/// The new scaled bitmap with the specified size and quality settings.
		/// </returns>
		/// Reference -> https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp
		public static Bitmap GetNewScaledBitmap(Bitmap bmp, float scale, BitmapResizeCriteria criteria) {
			scale = MathF.Abs(scale);
			var scaledWidth  = Math.Clamp((int)(bmp.Width  * scale), 1, MAX_BITMAP_DIMENSION) ;
			var scaledHeight = Math.Clamp((int)(bmp.Height * scale), 1, MAX_BITMAP_DIMENSION);

			var scaledBmp  = new Bitmap(scaledWidth, scaledHeight);
			var scaledRect = GetNewRect(ref scaledBmp);
			scaledBmp.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);

			using var graphics = Graphics.FromImage(scaledBmp);

			graphics.CompositingMode    = criteria.CompositingMode;
			graphics.CompositingQuality = criteria.CompositingQuality;
			graphics.InterpolationMode  = criteria.InterpolationMode;
			graphics.SmoothingMode      = criteria.SmoothingMode;
			graphics.PixelOffsetMode    = criteria.PixelOffsetMode;

			using var wrap = new ImageAttributes();

			wrap.SetWrapMode(WrapMode.TileFlipXY);
			graphics.DrawImage(bmp, scaledRect, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, wrap);

			return scaledBmp;
		}

		/// <summary>
		/// Releases the resources used by the BitmapParser class.
		/// </summary>
		public void Dispose() {
			if (IsDisposed)
				return;

			m_repository.Dispose();

			IsDisposed = true;
		}

		/// <summary>
		/// Modifies the RGB values of a Bitmap at a specific index in an unsafe and concurrent manner. 
		/// This code is base on: https://csharpexamples.com/fast-image-processing-c/
		///		from author Turgay
		/// Modifies the RGB values of a Bitmap at a specific index in an unsafe and concurrent manner.
		/// </summary>
		/// <param name="bitmapIndex">The index of the Bitmap in the internal array.</param>
		/// <param name="functor">A delegate function that performs the desired modifications on the pixel data.</param>
		/// <returns>Returns a copy of the bitmap at index bitMapIndex</returns>
		public unsafe Bitmap ModifyRgbUnsafeRef(int bitmapIndex, BitmapRgbDelegate functor) {
			if (IsDisposed)
				throw new BitMapParserDisposedException();

			var copy = new Bitmap(GetOriginal(bitmapIndex));
			// Lock the bitmap bits. This will allow us to modify the bitmap data.
			// Data is modified by traversing bitmap data (created in this method) and invoking functor.

			var bmpData = copy.LockBits(
				GetNewRect(ref copy),
				ImageLockMode.ReadWrite,
				copy.PixelFormat);

			// this gives us size in bits... divide by 8 to get size in bytes
			var   bytesPerPixel = Image.GetPixelFormatSize(copy.PixelFormat) / 8;
			var   imgHeight     = bmpData.Height;
			var   imgWidth      = bmpData.Width * bytesPerPixel;
			byte* headPtr       = (byte*)bmpData.Scan0;

			// note documentation for Parallel.For
			//		from INCLUSIVE ::: to EXCLUSIVE
			//		okay to pass 0, height
			Parallel.For(0, imgHeight, m_options.Get,
				pxlIndex => {
					byte* row = headPtr + pxlIndex * bmpData.Stride;

					for (var i = 0; i < imgWidth; i += bytesPerPixel) {
						// the current pixel to work on; passes rgb values to a delegate
						int red   = row[i + 2];
						int green = row[i + 1];
						int blue  = row[i];

						functor.Invoke(ref pxlIndex, ref red, ref green, ref blue);
						GuardRgbValue(ref red);
						GuardRgbValue(ref green);
						GuardRgbValue(ref blue);

						row[i + 2] = (byte)red;
						row[i + 1] = (byte)green;
						row[i]     = (byte)blue;
					}
				});

			copy.UnlockBits(bmpData);
			m_repository.Swap(bitmapIndex, copy);

			return copy;
		}

		/// <summary>
		/// Modifies the RGB values of multiple bitmaps using the provided functor.
		/// </summary>
		/// <param name="functor">The delegate that specifies how the RGB values should be modified.</param>
		/// <param name="bitmapIndices">The indices of the bitmaps to modify.</param>
		/// <returns>A reference to an array of modified bitmaps.</returns>
		public Bitmap[] ModifyRgbUnsafeRef(BitmapRgbDelegate functor, params int[] bitmapIndices) {
			foreach (var index in bitmapIndices) {
				var modified = ModifyRgbUnsafeRef(index, functor);
				m_repository.Swap(index, modified);
			}

			return m_repository.Modified;
		}

		/// <summary>
		/// Modifies the RGB values of all the bitmaps in the array using the specified functor
		/// </summary>
		/// <param name="functor">The delegate that defines the modification operation</param>
		/// <returns>A reference to the modified array of bitmaps</returns>
		public Bitmap[] ModifyAllRgbUnsafeRef(BitmapRgbDelegate functor) {
			for (var i = 0; i < m_repository.Count; i++) {
				var modified = ModifyRgbUnsafeRef(i, functor);
				m_repository.Swap(i, modified);
			}

			return m_repository.Modified;
		}

		/// <summary>
		/// Modifies the RGB values of a Bitmap at a specific index in an unsafe and concurrent manner. 
		/// This code is base on: https://csharpexamples.com/fast-image-processing-c/
		///		from author Turgay
		/// Modifies the RGB values of a Bitmap at a specific index in an unsafe and concurrent manner.
		/// This makes a call to an internally unsafe method
		/// </summary>
		/// <param name="bitmapIndex">The index of the Bitmap in the internal array.</param>
		/// <param name="functor">A delegate function that performs the desired modifications on the pixel data.</param>
		/// <returns>Returns reference to the internal array of Bitmap objects.</returns>
		public Bitmap ModifyRgbUnsafe(int bitmapIndex, BitmapRgbDelegate functor) {
			return ModifyRgbUnsafeRef(bitmapIndex, functor);
		}

		/// <summary>
		/// Modifies the RGB values of multiple bitmaps using the provided functor.
		/// </summary>
		/// <param name="functor">The delegate that specifies how the RGB values should be modified.</param>
		/// <param name="bitmapIndices">The indices of the bitmaps to modify.</param>
		/// <returns>A reference to an array of modified bitmaps.</returns>
		public Bitmap[] ModifyRgbUnsafe(BitmapRgbDelegate functor, params int[] bitmapIndices) {
			return ModifyRgbUnsafeRef(functor, bitmapIndices);
		}

		/// <summary>
		/// Modifies the RGB values of all the bitmaps in the array using the specified functor
		/// This calls an internally unsafe method.
		/// </summary>
		/// <param name="functor">The delegate that defines the modification operation</param>
		/// <returns>A reference to the modified array of bitmaps</returns>
		public Bitmap[] ModifyAllRgbUnsafe(BitmapRgbDelegate functor) {
			return ModifyAllRgbUnsafeRef(functor);
		}

		/// <summary>
		/// Asynchronously saves Bitmap objects to the files at a specified path.
		/// </summary>
		/// <param name="bitmaps">Bitmap array to save</param>
		/// <param name="path">The directory path where the Bitmaps should be saved.</param>
		/// <param name="disposeOnSuccess">Optional parameter determining whether to dispose the BitmapParser on successful save. Default is false.</param>
		/// <param name="format">Format to save each bitmap as.</param>
		/// <param name="token">Optional cancellation token</param>
		/// <param name="createPathIfNotExit">If true, will create the directory if it does not exist</param>
		/// <returns>Returns a Task that represents the asynchronous operation. The task result contains void.</returns>
		/// <exception cref="DirectoryNotFoundException">Thrown when the provided path is null, empty, or consists only of white-space characters.</exception>
		public async Task<List<string>> SaveBitmapsAsync(
			Bitmap[] bitmaps,
			string path,
			ImageFormat format,
			CancellationToken token,
			bool disposeOnSuccess = false,
			bool createPathIfNotExit = false) {
			if (IsDisposed)
				throw new BitMapParserDisposedException();

			if (string.IsNullOrWhiteSpace(path))
				throw new DirectoryNotFoundException(EXCEPTION_PATH_DOES_NOT_EXIST);

			if (createPathIfNotExit && !Directory.Exists(path))
				Directory.CreateDirectory(path);

			var rootPath = m_repository.GetPath(0);
			var output = await m_saver.SaveBitmapsAsync(
				             bitmaps, path, rootPath, format, token, disposeOnSuccess, createPathIfNotExit);

			if (disposeOnSuccess)
				Dispose();

			return output;
		}

		/// Validates the internal state of the BitmapParser instance.
		/// @throws BitMapParserDisposedException if the instance is disposed.
		/// /
		void ValidateInternalState() {
			if (IsDisposed)
				throw new BitMapParserDisposedException();
		}

		/// <summary>
		/// Guards the RGB value to ensure it is within the valid range.
		/// </summary>
		/// <remarks>
		/// If the <paramref name="value"/> is less than 0, it will be set to 0.
		/// If the <paramref name="value"/> is greater than 255, it will be set to 255.
		/// </remarks>
		/// <param name="value">The RGB value to be guarded.</param>
		/// <returns>None.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static void GuardRgbValue(ref int value) {
			if (value < 0)
				value = 0;

			if (value > 255)
				value = 255;
		}

		/// <summary>
		/// Creates a new <see cref="Rectangle"/> object with the specified dimensions based on the provided <see cref="Bitmap"/>.
		/// </summary>
		/// <param name="bmp">The <see cref="Bitmap"/> used to determine the width and height of the rectangle.</param>
		/// <returns>A new <see cref="Rectangle"/> object with the specified dimensions.</returns>
		static Rectangle GetNewRect(ref Bitmap bmp) => new(0, 0, bmp.Width, bmp.Height);

#region PLUMBING

		public BitmapParser(IBitmapRepository bitmapRepository, IImageSaver imageSaver, IParallelOptions options) {
			m_repository = bitmapRepository;
			m_saver      = imageSaver;
			m_options    = options;
		}

		readonly IBitmapRepository m_repository;
		readonly IImageSaver       m_saver;
		readonly IParallelOptions  m_options;

#endregion

#region EXCEPTION_CONSTANTS

		internal const string EXCEPTION_PATH_DOES_NOT_EXIST = "Supplied path does not exist.";
		internal const int    MAX_BITMAP_DIMENSION          = 50000;

#endregion
	}
}