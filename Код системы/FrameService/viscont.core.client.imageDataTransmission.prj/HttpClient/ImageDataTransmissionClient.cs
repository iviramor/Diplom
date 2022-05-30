using System;
using System.Threading.Tasks;
using System.Net.Http;

using Microsoft.AspNetCore.SignalR.Client;

using Mallenom.Framework;
using Mallenom.Imaging;

using Viscont.Core.Framework.ImageDataTransmission;

namespace Viscont.Core.Client.ImageDataTransmission;

public class ImageDataTransmissionClient : IImageDataTransmissionClient, IDisposable
{
	#region Data

	private const string BaseLocalUrl  = "http://localhost:61715/";

	private const string ImageUri = "Image/";
	//TODO: Изменить
	private const string SendUri  = "send/";
	private const string GetUrl   = "get/";

	private const string ImageHub = "NewImage/";

	private const string NewImageMethod    = "NewImage";
	private const string RemoveImageMethod = "RemoveImage";

	private readonly HttpClient _httpClient;

	private readonly IImageDataWriter _writer;
	//private readonly IImageDataReader _reader;

	#endregion

	#region .ctor

	public ImageDataTransmissionClient()
	{
		_httpClient = new()
		{
			BaseAddress = new Uri(BaseLocalUrl)
		};
		_writer = new ImageDataWriter();
	}

	public void Dispose()
	{
		_httpClient.Dispose();
	}

	#endregion

	#region Implementation

	public async Task<Guid> SendImageAsync(IReadOnlyReference<ImageData> imageReference)
	{
		var guid = Guid.NewGuid();

		var url = ImageUri + SendUri + guid;

		//TODO: Изменить
		using var writer = _writer.WriteImageToMemory(guid, imageReference.Value!);
		HttpResponseMessage hrm = await _httpClient.GetAsync(url);
		hrm.Dispose();

		return guid;
	}

	public async Task<HubConnection> SubscribeOnNewImage(Action<Guid> action)
	{
		var hubConnection = new HubConnectionBuilder()
			.WithUrl(BaseLocalUrl + ImageHub)
			.Build();

		hubConnection.On<Guid>(NewImageMethod, message => action(message));

		await hubConnection.StartAsync();

		return hubConnection;
	}

	#endregion
}
