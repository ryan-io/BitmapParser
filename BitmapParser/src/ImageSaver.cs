// BitmapParser

using System.Drawing;
using System.Drawing.Imaging;

namespace BitmapParser;

public class ImageSaver : IImageSaver {
	public async Task<List<string>> SaveBitmapsAsync(
		Bitmap[] bitmaps,
		string path, 
		string rootPath,
		ImageFormat format, 
		CancellationToken token, 
		bool disposeOnSuccess = false,
		bool createPathIfNotExit = false) {
		
		var output = new List<string>();
		var count  = bitmaps.Length;
		var tasks  = new Task[count];

		for (var i = 0; i < count; i++) {
			
			var sanitizedPath = GetSanitizedPath(ref path, ref rootPath);
			output.Add(sanitizedPath);
			tasks[i] = SaveImageTask(bitmaps[i], sanitizedPath, format, token);
		}

		await Task.WhenAll(tasks);
		return output;
	}

	/// <summary>
	/// Gets a sanitized path by combining the given path with the filename without extension of the path at index 0, and appends "_bmpParsed.png" extension to it.
	/// </summary>
	/// <param name="path">The path to be sanitized. This parameter is passed by reference and will be modified to the sanitized path.</param>
	/// <param name="rootPath">Root path to get the filename from</param>
	/// <returns>The sanitized path.</returns>
	string GetSanitizedPath(ref string path, ref string rootPath) {
		var newPath = Path.Combine(path, Path.GetFileNameWithoutExtension(rootPath) + "_bmpParsed.png");
		return newPath;
	}
	
	
	/// <summary>
	/// Saves an image to the specified path asynchronously.
	/// </summary>
	/// <param name="bmp">The image to be saved.</param>
	/// <param name="path">The path where the image will be saved.</param>
	/// <param name="format">Format to save each image as.</param>
	/// <param name="token">Cancellation token for cancelling async state machine</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	static Task SaveImageTask(Image bmp, string path, ImageFormat format, CancellationToken token)
		=> Task.Run(() => bmp.Save(path, format), token);
}