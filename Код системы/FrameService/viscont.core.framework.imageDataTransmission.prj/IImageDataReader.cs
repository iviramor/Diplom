using System;
using Mallenom.Framework;
using Mallenom.Imaging;

namespace Viscont.Core.Framework.ImageDataTransmission;

public interface IImageDataReader
{
	void ReadImageFromMemory(
		Guid imageId,
		Reference<ImageData> reference);
}
