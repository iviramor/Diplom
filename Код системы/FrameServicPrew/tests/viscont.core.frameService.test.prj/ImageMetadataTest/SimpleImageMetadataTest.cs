using System;

using NUnit.Framework;

using Mallenom.Imaging;

using Viscont.Core.Framework.ImageDataTransmission;

namespace Viscont.Core.FrameService.Test;

public class SimpleImageMetadataTest
{
	#region Test data

	private string _stringNameFile;
	private ImageMetadata _imageMetadata;

	#endregion

	[SetUp]
	public void SetUp()
	{
		var guid		= Guid.NewGuid();
		var format		= ImageDataFormat.Y8.ToString();
		var height		= 100;
		var width		= 100;
		var fileFormat	= "bmp";

		_stringNameFile = @$"{guid:N}-{format}-{width}x{height}.{fileFormat}";
		_imageMetadata	= new ImageMetadata(
										ImageName:		guid.ToString("N"),
										ImageFormat:	format,
										ImageHeight:	height,
										ImageWidth:		width,
										ImageFileType:	fileFormat);
	}

	[Test]
	public void SimpleImageMetadataFromStringTest()
	{
		var imageMetaData = ImageMetadata.GetImageMetadataFromString(_stringNameFile);
		Assert.Multiple(() =>
		{
			Assert.That(_imageMetadata.ImageName,   Is.EqualTo(imageMetaData.ImageName));
			Assert.That(_imageMetadata.ImageFormat, Is.EqualTo(imageMetaData.ImageFormat));
		});
	}

	[Test]
	public void SimpleImageMetadataFromMetadataTest()
	{
		var imageMetaData = ImageMetadata.GetStringImageNameFromImageMetadata(_imageMetadata);
		Assert.That(_stringNameFile, Is.EqualTo(imageMetaData));
	}
}
