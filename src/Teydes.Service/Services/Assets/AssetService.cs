using AutoMapper;
using Teydes.Data.IRepositories;
using Teydes.Domain.Entities.Assets;
using Teydes.Service.Commons.Helpers;
using Teydes.Service.Interfaces.Assets;
using Teydes.Service.Commons.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Teydes.Service.Services.Assets;

public class AssetService : IAssetService
{
    private readonly IRepository<Asset> assetRepository;

    public AssetService(IRepository<Asset> assetRepository)
    {
        this.assetRepository = assetRepository;
    }
    public async Task<bool> RemoveAsync(long id)
    {
        var asset = await this.assetRepository.SelectAsync(a => a.Id == id);
        if (asset is null)
            throw new CustomException(404, "Attachment not found");

        string rootPath = EnvironmentHelper.WebRootPath;
        string imagePath = Path.Combine(rootPath,asset?.Path);
        if (File.Exists(imagePath))
            File.Delete(imagePath);
        var result = await this.assetRepository.DeleteAsync(id);
        await this.assetRepository.SaveAsync();

        return result;
    }

    public async Task<Asset> RetriveByIdAsync(long id)
    {
        var asset = await this.assetRepository.SelectAll()
            .Where(a => a.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        if (asset is null)
            throw new CustomException(404, "Asset is not found");

        return asset;
    }

    public async Task<Asset> UploadAsync(IFormFile file)
    {
        // combining paths and create if not exists
        string rootPath = Path.Combine(EnvironmentHelper.WebRootPath, "Files");
        string fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
        string path = Path.Combine(rootPath,"assets", fileName);
    
        using (var fileStream = File.OpenWrite(path))
        {
            await file.CopyToAsync(fileStream);
        }

        var asset = new Asset()
        {
            CreatedAt = DateTime.UtcNow,
            Path = Path.Combine("Files", "assets", fileName)
        };
        var result = await this.assetRepository.InsertAsync(asset);
       
        await this.assetRepository.SaveAsync();

        return result;
    }
}
