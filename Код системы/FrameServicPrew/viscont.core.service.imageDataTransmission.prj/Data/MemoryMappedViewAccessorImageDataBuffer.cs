using Mallenom.Imaging;
using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viscont.Core.Service.ImageDataTransmission.Data
{
	public class MemoryMappedViewAccessorImageDataBuffer : IImageDataBuffer
	{
		public IntPtr Pointer { get; }
		public int Size { get; }

		private readonly MemoryMappedViewAccessor _accessor;

		public unsafe MemoryMappedViewAccessorImageDataBuffer(
			MemoryMappedViewAccessor accessor,
			int bufferSize)
		{
			byte* ptr = null;

			_accessor = accessor;
			_accessor.SafeMemoryMappedViewHandle.AcquirePointer(ref ptr);

			Pointer = new IntPtr(ptr);
			Size = bufferSize;
		}

		public void Dispose()
		{
			_accessor.SafeMemoryMappedViewHandle.ReleasePointer();
			_accessor.Dispose();
		}
	}
}
