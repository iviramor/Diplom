using System;

using Viscont.Core.Service.ImageDataTransmission.Data;

namespace Viscont.Core.Service.ImageDataTransmission.Data;

public interface IImageRepository
{
	/// <summary> Отдает количество файлов в репозитории </summary>
	int GetCount();

	/// <summary> Добавление изображения </summary>
	void Add(Guid imageId, ImageEntry imageMetadataRepositoty);

	/// <summary> Удаление </summary>
	bool Remove(Guid imageId);

	/// <summary> Отдает Данные изображения репозитория по Guid </summary>
	ImageEntry GetByGuid(Guid guid);

	bool TryGetByGuid(Guid imageId, out ImageEntry entry);
}
