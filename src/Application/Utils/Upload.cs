using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Core.Utils;

public class Upload
{
    private readonly IHostEnvironment _hostingEnvironment;

    public Upload(IHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task AddImageAsync(Produto produto, IFormFile uploadImage, string? path,
        CancellationToken cancellationToken)
    {
        const string imagesFolder = "images";
        var fullPath = !string.IsNullOrEmpty(path) ? path + "/" + imagesFolder : imagesFolder;
        var uploads = Path.Combine(_hostingEnvironment.ContentRootPath, fullPath);
        var exists = Directory.Exists(uploads);

        if (!exists)
            Directory.CreateDirectory(uploads);
        var extension = uploadImage.FileName.Split('.');
        var filename = $"{Guid.NewGuid()}.{extension[1]}";
        var filePath = Path.Combine(uploads, filename);
        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
        {
            await uploadImage.CopyToAsync(fileStream, cancellationToken);
            produto.Imagem = filename;
        }
    }
}