using Mallenom;
using Mallenom.Imaging;

namespace Viscont.Core.Framework.ImageDataTransmission;

public record class ImageMetadata(
	string          ImageName,
	ImageDataFormat ImageFormat,
	int             ImageWidth,
	int             ImageHeight,
	string          ImageFileType);
