// BitmapParser

namespace BitmapParser;

/// <summary>
/// Represents the available image types.
/// </summary>
[Serializable, Flags]
public enum ImageType {
	PNG  = 1 << 0,
	JPG  = 1 << 1,
	JPEG = 1 << 2,
	BMP  = 1 << 3,
	ALL  = 1 << 4
}