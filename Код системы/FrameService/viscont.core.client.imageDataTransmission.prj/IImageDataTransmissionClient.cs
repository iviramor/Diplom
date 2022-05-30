using System.Threading.Tasks;
using System;

using Microsoft.AspNetCore.SignalR.Client;

using Mallenom.Imaging;
using Mallenom.Framework;

namespace Viscont.Core.Client.ImageDataTransmission;

public interface IImageDataTransmissionClient
{
	/// <summary> Отправка изображения </summary>
	/// <returns> Guid </returns>
	Task<Guid> SendImageAsync(
		IReadOnlyReference<ImageData> imageReference);

	Task<HubConnection> SubscribeOnNewImage(
		Action<Guid> action);
}
