
using Teydes.Domain.Entities.Users;
using Teydes.Service.DTOs;

namespace Teydes.Service.Interfaces.Users;

public interface ISmsService
{
    public Task<bool> SendAsync(Message message);
    public Task<string> GenerateTokenAsync();
    public Task<bool> SendMessageByTelegramAsync(long groupId, string url, string text);
}
