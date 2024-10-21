using Teydes.Domain.Configurations;
using Teydes.Service.DTOs.UserGroups;

namespace Teydes.Service.Interfaces.UserGroups;

public interface IUserGroupService
{
    Task<bool> RemoveAsync(long id);
    IEnumerable<UserGroupForResultDto> GetAll();
    Task<UserGroupForResultDto> RetrieveByIdAsync(long id);
    Task<UserGroupForResultDto> AddAsync(UserGroupForCreationDto dto);
    Task<UserGroupForResultDto> ModifyAsync(long id, UserGroupForUpdateDto dto);
    Task<IEnumerable<UserGroupForResultDto>> RetrieveAllAsync(PaginationParams @params);
}
