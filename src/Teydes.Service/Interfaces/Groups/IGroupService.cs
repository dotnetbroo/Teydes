using Teydes.Service.DTOs.Groups;
using Teydes.Domain.Configurations;

namespace Teydes.Service.Interfaces.Groups;

public interface IGroupService
{

    Task<bool> RemoveAsync(long id);
    IEnumerable<GroupForResultDto> GetAll();
    Task<GroupForResultDto> RetrieveByIdAsync(long id);
    Task<GroupForResultDto> AddAsync(GroupForCreationDto dto);
    Task<GroupForResultDto> ModifyAsync(long id, GroupForUpdateDto dto);
    Task<IEnumerable<GroupForResultDto>> RetrieveAllAsync(PaginationParams @params);
    Task<GroupForStudentRankingResultDto> RetrieveByIdWithStudentRankingAsync(long id);
    Task<IEnumerable<GroupForResultDto>> SearchAllAsync(string search, PaginationParams @params);
}
