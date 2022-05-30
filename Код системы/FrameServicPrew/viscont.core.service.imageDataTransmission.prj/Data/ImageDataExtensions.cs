using Mallenom.Imaging;

using System.IO.MemoryMappedFiles;

namespace Viscont.Core.Service.ImageDataTransmission.Data;

public static class ImageDataExtensions
{
	public unsafe static ImageData RepresentAsImageData(this ImageEntry imageEntry)
	{
		int bufferSize = ImageDataLayout.GetRequiredCapacity(
			imageEntry.ImageMetadata.ImageFormat,
			imageEntry.ImageMetadata.ImageWidth,
			imageEntry.ImageMetadata.ImageHeight);

		var reader = imageEntry.MemoryMappedFile
			.CreateViewAccessor(0, bufferSize, MemoryMappedFileAccess.Read);
		var imageBuffer = new MemoryMappedViewAccessorImageDataBuffer(reader, bufferSize);

		var layout = ImageDataLayout.Create(
			imageBuffer.Pointer,
			imageEntry.ImageMetadata.ImageFormat,
			imageEntry.ImageMetadata.ImageWidth,
			imageEntry.ImageMetadata.ImageHeight);

		var src = new ImageData(
			imageBuffer,
			layout.Slice0,
			layout.Slice1,
			layout.Slice2,
			imageEntry.ImageMetadata.ImageWidth,
			imageEntry.ImageMetadata.ImageHeight,
			imageEntry.ImageMetadata.ImageFormat);

		return src;
	}
}
