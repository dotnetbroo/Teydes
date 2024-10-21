using Teydes.Service.DTOs.Courses;
using Teydes.Domain.Configurations;

namespace Teydes.Service.Interfaces.Courses;

public interface ICourseService
{
    Task<bool> RemoveAsync(long id);
    Task<IEnumerable<CourseForResultDto>> GetAllAsync();
    Task<CourseForResultDto> RetrieveByIdAsync(long id);
    Task<CourseForResultDto> AddAsync(CourseForCreationDto dto);
    Task<CourseForResultDto> ModifyAsync(long id, CourseForUpdateDto dto);
    Task<IEnumerable<CourseForResultDto>> RetrieveAllAsync(PaginationParams @params);
    Task<IEnumerable<CourseForResultDto>> SearchAllAsync(string search, PaginationParams @params);
}
