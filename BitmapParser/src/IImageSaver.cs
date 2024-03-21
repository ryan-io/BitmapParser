// BitmapParser

using System.Drawing;
using System.Drawing.Imaging;

namespace BitmapParser;

public interface IImageSaver {
	Task<List<string>> SaveBitmapsAsync(
		Bitmap[] bitmaps,
		string path,
		string rootPath,
		ImageFormat format,
		CancellationToken token,
		bool disposeOnSuccess = false,
		bool createPathIfNotExit = false);
}