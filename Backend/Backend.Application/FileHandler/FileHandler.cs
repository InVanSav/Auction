using Backend.Domain.Entity;
using FluentResults;
using Microsoft.AspNetCore.Http;

namespace Backend.Application.FileHandler;

/// <summary>
/// Файл для работы с изображениями (чтение/запись/удаление)
/// </summary>
public class FileHandler
{
    /// <summary>
    /// Получение изображений с локальной машины/сервера
    /// </summary>
    /// <param name="lotName">Название лота</param>
    /// <returns></returns>
    public async Task<IEnumerable<object>?> GetImagesFromHost(string lotName)
    {
        var imagesData = new List<object>();

        var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "../Backend.Images", lotName);
        if (!Directory.Exists(imagesPath)) return null;

        var imageFiles = Directory.GetFiles(imagesPath);

        foreach (var imageFile in imageFiles)
        {
            var imageBytes = await File.ReadAllBytesAsync(imageFile);
            var base64Image = Convert.ToBase64String(imageBytes);
            var imageName = Path.GetFileName(imageFile);

            var imageData = new { name = imageName, data = base64Image };
            imagesData.Add(imageData);
        }

        return imagesData;
    }

    /// <summary>
    /// Сохранение изображений на локальную машину/сервер
    /// </summary>
    /// <param name="imageFiles">Файлы изображений</param>
    /// <param name="lotId">Уникальный идентификатор лота</param>
    /// <param name="lotName">Название лота</param>
    /// <returns></returns>
    public async Task<IEnumerable<Image>?> SaveImagesToHostAsync(
        IEnumerable<IFormFile> imageFiles, Guid lotId, string lotName)
    {
        var number = 0;
        var images = new List<Image>();
        var imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "../Backend.Images", lotName);

        foreach (var image in imageFiles)
        {
            if (image.Length <= 0) return null;

            var fileName = $"{number}{Path.GetExtension(image.FileName)}";
            var absolutePath = Path.Combine(imagesDirectory, fileName);

            if (!Directory.Exists(imagesDirectory)) Directory.CreateDirectory(imagesDirectory);

            await using var stream = new FileStream(absolutePath, FileMode.Create);
            await image.CopyToAsync(stream);

            number++;

            images.Add(new Image
            {
                Id = Guid.NewGuid(),
                LotId = lotId,
                Path = absolutePath
            });
        }

        return images;
    }

    /// <summary>
    /// Удаление изображений с локальной машины/сервера
    /// </summary>
    /// <param name="lotName">Название лота</param>
    /// <returns></returns>
    public async Task DeleteImagesFromHostAsync(string lotName)
    {
        var imagesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "../Backend.Images", lotName);

        if (!Directory.Exists(imagesFolderPath)) return;

        await Task.Factory.StartNew(path =>
            Directory.Delete(path?.ToString()!, true), imagesFolderPath);
    }
}