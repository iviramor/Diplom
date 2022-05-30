using System;
using System.IO.MemoryMappedFiles;

using Mallenom.Imaging;

namespace Viscont.Core.Framework.ImageDataTransmission;

public class ImageDataWriter : IImageDataWriter
{
	#region Implementation

	public IDisposable WriteImageToMemory(
		Guid guid,
		ImageData imageData)
	{
		var imageSize  = ImageDataLayout.GetRequiredCapacity(imageData.Format, imageData.Width, imageData.Height);
		var memoryMappedFile = MemoryMappedFile.CreateNew(guid.ToString("N"), imageSize);
		using var writer = memoryMappedFile.CreateViewAccessor(0, imageSize);
			WriteToMemory(imageData, writer);
		return memoryMappedFile;
	}

	#endregion

	#region Methods

	private static unsafe void WriteToMemory(
		ImageData imageData,
		MemoryMappedViewAccessor writer)
	{
		byte* ptr = null;
		writer.SafeMemoryMappedViewHandle.AcquirePointer(ref ptr);

		try
		{
			var layout = ImageDataLayout.Create(
				(IntPtr)ptr,
				imageData.Format,
				imageData.Width,
				imageData.Height);

			using var data = new ImageData(
				layout.Slice0,
				layout.Slice1,
				layout.Slice2,
				imageData.Width,
				imageData.Height,
				imageData.Format);
			ColorSpaceConverter.Convert(imageData, data);
		}
		finally
		{
			writer.SafeMemoryMappedViewHandle.ReleasePointer();
		}
	}

	#endregion
}
