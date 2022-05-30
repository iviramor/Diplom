using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viscont.Core.Framework.ImageDataTransmission;

public record class ImageMetadataModel(
	string FileName,
	int Width,
	int Height,
	string Format,
	string FileFormat);
