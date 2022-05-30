using System;

using NUnit.Framework;

using Mallenom.Framework;
using Mallenom.Imaging;

using Viscont.Core.Framework.ImageDataTransmission;

namespace Viscont.Core.FrameService.Test;

public class SimpleWriteReadImageTest
{
	#region Data

	private Reference<ImageData> _imageReference;
	private ImageMetadata _imageMetadata;

	private IImageDataWriter _writer;
	private IImageDataReader _reader;

	#endregion

	[SetUp]
	public void SetUp()
	{
		var fileName    = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Data", "testPicture.bmp");
		var matrix		= Matrix.LoadFrom(fileName);
		_imageReference = Reference.FromInstance(matrix.LockData());

		var guid		= Guid.NewGuid();
		_imageMetadata	= new(
			ImageName:		guid.ToString("N"),
			ImageFormat:	_imageReference.Value!.Format.Name,
			ImageHeight:	_imageReference.Value!.Height,
			ImageWidth:		_imageReference.Value!.Width,
			ImageFileType: "bmp");

		_writer = new ImageDataWriter();
		_reader = new ImageDataReader(new ImageDataAllocator());
	}

	[Test]
	public void SimpleImageDataWriteTest()
		=> Assert.That(() => _writer.WriteImageToMemory(Guid.NewGuid(), _imageReference.Value!), Throws.Nothing);

	[Test]
	public void SimpleImageDataReadTest()
	{
		var guid = Guid.NewGuid();

		using(_writer.WriteImageToMemory(guid, _imageReference.Value!))
		{
			using var reference = new Reference<ImageData>();
			_reader.ReadImageFromMemory(guid, reference);

			Assert.That(reference.Value, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(reference.Value.Width,  Is.EqualTo(_imageMetadata.ImageWidth));
				Assert.That(reference.Value.Height, Is.EqualTo(_imageMetadata.ImageHeight));
			});
		}
	}
}
