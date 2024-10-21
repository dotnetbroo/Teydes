using Microsoft.AspNetCore.Http;
using Teydes.Domain.Entities.Assets;

namespace Teydes.Service.Interfaces.Assets;

public interface IAssetService
{
    Task<Asset> UploadAsync(IFormFile file);
    Task<bool> RemoveAsync(long id);
    Task<Asset> RetriveByIdAsync(long id);
}
