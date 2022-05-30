using Mallenom.Framework;
using Mallenom.Imaging;
using System;
using System.IO;

using System.Net.Http;
using System.Threading.Tasks;

using Viscont.Core.Framework.ImageDataTransmission;

namespace viscont.core.frameService.consoleTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {


			using (var client = new HttpClient() { BaseAddress = new("http://localhost:5000/Image/") })
			{
				Guid guid = Guid.NewGuid();

				var matrix = Matrix.LoadFrom(@"C:\Users\Workspace\Mallenom\Viscont\core\src\FrameService\tests\viscont.core.frameService.consoleTest\Data\testPicture.bmp");
				using var imageReference = Reference.FromInstance(matrix.LockData());
				ImageDataWriter writer = new ImageDataWriter();
				using (writer.WriteImageToMemory(guid, imageReference.Value))
				{
					using var res = await client.GetAsync($"save?fileName={guid:N}&w={imageReference.Value.Width}&h={imageReference.Value.Height}&format={imageReference.Value.Format.Name}");
					res.EnsureSuccessStatusCode();
					var g = await res.Content.ReadAsStringAsync();
					guid = Guid.Parse(g[1..^1]);
				}

				var stream = await client.GetStreamAsync($"get?guid={guid}&format=jpeg");

				using var fs = new FileStream("out.jpeg", FileMode.Create);
				await stream.CopyToAsync(fs);

			}
        }
    }
}
