using Mallenom.Framework;
using Mallenom.Imaging;

using System;

namespace Viscont.Core.Framework.ImageDataTransmission;

public interface IImageDataWriter
{
	IDisposable WriteImageToMemory(
		Guid imageId,
		ImageData imageDataReference);
}
