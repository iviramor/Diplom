using System;
using System.IO.MemoryMappedFiles;

using Mallenom.Framework;
using Mallenom.Imaging;

namespace Viscont.Core.Framework.ImageDataTransmission;

public class ImageDataReader : IImageDataReader
{
	#region Data

	private readonly IImageDataAllocator _imageDataAllocator;

	#endregion

	#region .ctor

	public ImageDataReader(IImageDataAllocator imageDataAllocator)
	{
		_imageDataAllocator = imageDataAllocator
			?? throw new ArgumentNullException(nameof(imageDataAllocator));
	}

	#endregion

	#region Implementation
	public void ReadImageFromMemory(
		Guid imageId,
		Reference<ImageData> reference)
	{
		var imageData = reference.Value;

		int sizeImage = ImageDataLayout.GetRequiredCapacity(imageData!.Format,
			imageData.Width, imageData.Height);

		using var sharedMemory = OperatingSystem.IsWindows()
			? MemoryMappedFile.OpenExisting(imageId.ToString("N"))
			: throw new PlatformNotSupportedException();
		using var reader = sharedMemory.CreateViewAccessor(0, sizeImage, MemoryMappedFileAccess.Read);
		ReadFromMemory(reader, reference);
	}

	#endregion

	#region Methods

	private unsafe void ReadFromMemory(
		MemoryMappedViewAccessor reader,
		Reference<ImageData> reference)
	{
		byte* ptr = null;

		reader.SafeMemoryMappedViewHandle.AcquirePointer(ref ptr);

		try
		{
			if(!_imageDataAllocator.TryAllocate(
				reference,
				reference.Value!.Format,
				reference.Value!.Width,
				reference.Value!.Height))
			{
				throw new Exception("_imageDataAllocator.TryAllocate return false");
			}
			try
			{

				var layout = ImageDataLayout.Create(
												(IntPtr)ptr,
												reference.Value!.Format,
												reference.Value!.Width,
												reference.Value!.Height);

				using var src = new ImageData(
											layout.Slice0,
											layout.Slice1,
											layout.Slice2,
											reference.Value!.Width,
											reference.Value!.Height,
											reference.Value!.Format);
				ColorSpaceConverter.Convert(src, reference.Value!);
			}
			catch
			{
				reference.UnreferenceValue();
				throw;
			}
		}
		finally
		{
			reader.SafeMemoryMappedViewHandle.ReleasePointer();
		}
	}

	#endregion
}
