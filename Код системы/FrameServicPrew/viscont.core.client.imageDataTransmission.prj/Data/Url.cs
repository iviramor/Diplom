using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viscont.Core.Client.ImageDataTransmission;

/// <summary>
/// Адресация сервиса <br/><br/>
/// 
/// ImagesInfo/Get/Count/ <br/><br/>
/// 
/// Images/Get/Image/ <br/>
/// Images/Save/Image <br/>
/// Images/Remove/Image <br/><br/>
/// 
/// Images/Get/ImageMetadata/ <br/>
/// </summary>
public static class Url
{
	public const string BaseLocalUrl = "http://localhost:61715/";
	public const string BaseUrl      = "http://localhost:5000/";

	#region Image

	public const string ImagesUrl     = "Images/";
	public const string ImagesInfoUrl = "ImagesInfo/";

	public const string GetUrl    = "Get/";
	public const string SaveUrl   = "Save/";
	public const string RemoveUrl = "Remove/";

	public const string Count         = "Count/";
	public const string Image         = "Image/";
	public const string ImageMetadata = "ImageMetadata/";

	//Hubs methods
	public const string NewImageMethod    = "NewImage";
	public const string RemoveImageMethod = "RemoveImage";

	#endregion
}
