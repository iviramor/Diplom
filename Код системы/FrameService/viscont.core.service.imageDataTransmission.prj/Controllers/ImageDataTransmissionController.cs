using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;

using Mallenom.Framework;
using Mallenom.Imaging;

using System;
using System.Text.Json;
using System.IO;
using System.IO.MemoryMappedFiles;

using Viscont.Core.Framework.ImageDataTransmission;

using Viscont.Core.Service.ImageDataTransmission.Data;
using Viscont.Core.Service.ImageDataTransmission.Hubs;
using System.Threading.Tasks;
using Mallenom.Imaging.Jpeg;

namespace Viscont.Core.Service.ImageDataTransmission.Controllers;

[ApiController]
[Route("Image")]
public class ImageDataTransmissionController : ControllerBase
{
	#region Data

	private readonly IImageRepository _imageRepository;
	private readonly ILogger<ImageDataTransmissionController> _log;

	#endregion

	#region .ctor

	public ImageDataTransmissionController(
		IImageRepository imageRepository,
		IHubContext<NotificationHub> hub,
		ILogger<ImageDataTransmissionController> logger)
	{
		_imageRepository = imageRepository;
		_log			 = logger;
	}

	#endregion

	#region GET Get image

	/// <summary> Отдает количество с репозитория </summary>
	[HttpGet("count")]
	public int GetCount() => _imageRepository.GetCount();

	/// <summary> Отдает изображение с репозитория </summary>
	[HttpGet]
	[Route("get/image")]
	public IActionResult GetImage(Guid guid, string format)
	{
		if(!_imageRepository.TryGetByGuid(guid, out var imageEntry))
		{
			return NotFound();
		}

		using var imageData = imageEntry.RepresentAsImageData();

		var stream = new MemoryStream();

		switch (format)
		{
			case "bmp":
				BmpImage.Write(imageData, stream);
				break;
			case "jpg" or "jpeg":
				JpegImage.Encode(imageData, stream);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(format));
		}

		stream.Seek(0, SeekOrigin.Begin);

		return Ok(stream);
	}

	/// <summary> Отдает данные изображения с репозитория </summary>
	[HttpGet]
	[Route("get/ImageMetadata")]
	public IActionResult GetImageMetadata(Guid guid)
		=> _imageRepository.TryGetByGuid(guid, out var imageEntry) ? Ok(imageEntry.ImageMetadata) : NotFound();

	#endregion

	#region GET Send image

	/// <summary> Считывает и сохраняет в репозиторий файл из памяти </summary>
	[HttpGet]
	[Route("save")]
	public Guid SaveImage(string fileName, int w, int h, string format)
	{
		_log.LogInformation(fileName);

		var guid = Guid.NewGuid();

		var dataFormat = ImageDataFormat.GetFormatByName(format)
				?? throw new Exception($"Format not found. Source: {this}"); //TODO: Описание

		var memoryMappedFile = OperatingSystem.IsWindows()
			? MemoryMappedFile.OpenExisting(fileName)
			: throw new PlatformNotSupportedException();

		try
		{
			var metaData = new ImageMetadata(fileName, dataFormat, w, h, string.Empty);

			_imageRepository.Add(
				guid,
				new ImageEntry(metaData, memoryMappedFile));

			_log.LogInformation(guid.ToString());

			return guid;
		}
		catch
		{
			memoryMappedFile.Dispose();
			throw;
		}
	}

	#endregion

	#region GET Remove image

	/// <summary> Удаляет файл из репозиторий </summary>
	[HttpDelete]
	[Route("remove")]
	public IActionResult RemoveImage(Guid guid)
		=> _imageRepository.Remove(guid) ? Ok() : NotFound();

	#endregion
}
