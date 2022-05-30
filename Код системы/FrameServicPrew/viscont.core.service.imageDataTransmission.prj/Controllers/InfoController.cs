using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Viscont.Core.Service.ImageDataTransmission.Data;

namespace Viscont.Core.Service.ImageDataTransmission.Controllers;

[ApiController]
[Route("ImagesInfo")]
public class InfoController
{
	#region Data

	private readonly IImageRepository _imageRepository;
	private readonly ILogger<ImageDataTransmissionController> _log;

	#endregion

	#region .ctor

	public InfoController(
		IImageRepository imageRepository,
		ILogger<ImageDataTransmissionController> logger)
	{
		_imageRepository = imageRepository;
		_log = logger;
	}

	#endregion

	#region GET Get image

	/// <summary> Отдает количество с репозитория </summary>
	[HttpGet("Get/Count")]
	public int GetCount() => _imageRepository.GetCount();

	#endregion
}
