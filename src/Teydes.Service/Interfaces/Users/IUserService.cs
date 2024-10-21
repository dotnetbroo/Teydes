using Teydes.Service.DTOs.Users;
using Teydes.Domain.Configurations;
using Teydes.Domain.Entities.Users;

namespace Teydes.Service.Interfaces.Users;

public interface IUserService
{
    Task<bool> RemoveAsync(long id);
    Task<bool> TelegramIdExistsAsync(long TelegramId);
    Task<UserForResultDto> RetrieveByIdAsync(long id);
    Task<UserForResultDto> AddAsync(UserForCreationDto dto);
    Task<IEnumerable<UserForResultDto>> GetAllByGroupAsync(long id);
    Task<UserForResultDto> ModifyTelegramId(long id, long telegramId);
    Task<UserForResultDto> ModifyAsync(long id, UserForUpdateDto dto);
    public Task<UserForResultDto> RetrieveByIdWithQuizzesAsync(long id);
    Task<bool> ChangePasswordAsync(long id, UserForChangePasswordDto dto);
    Task<UserForResultDto> RetrieveByPhoneNumberAsync(string phoneNumber);
    Task<IEnumerable<UserForResultDto>> RetrieveAllAsync(PaginationParams @params);
    Task<bool> SendMessageByTelegramIdAsync(long TelegramId, string text, string url);
    Task<IEnumerable<UserForResultDto>> RetrieveAllAdminsAsync(PaginationParams @params);
    Task<IEnumerable<UserForResultDto>> RetrieveAllTeachersAsync(PaginationParams @params);
    Task<IEnumerable<UserStatisticsForResultDto>> RetrieveAllStudentsWithStatisticsAsync();
    Task<IEnumerable<UserForResultDto>> SearchAllAsync(string search, PaginationParams @params);
    Task<IEnumerable<UserForResultDto>> SearchAdminsAsync(string search, PaginationParams @params);
    Task<bool> ForgetPasswordAsync(string PhoneNumber, string NewPassword, string ConfirmPassword);
    Task<IEnumerable<UserForResultDto>> SearchTeachersAsync(string search, PaginationParams @params);
    Task<IEnumerable<UserStatisticsForResultDto>> RetrieveAllStudentsWithStatisticsAsync(PaginationParams @params);
}
