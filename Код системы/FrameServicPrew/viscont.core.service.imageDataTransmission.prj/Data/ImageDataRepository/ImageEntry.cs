using System.IO.MemoryMappedFiles;
using Viscont.Core.Framework.ImageDataTransmission;

namespace Viscont.Core.Service.ImageDataTransmission.Data;

public sealed record class ImageEntry(
	ImageMetadata ImageMetadata,
	MemoryMappedFile MemoryMappedFile);
