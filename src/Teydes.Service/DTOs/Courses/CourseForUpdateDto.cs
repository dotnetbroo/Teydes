using Teydes.Domain.Commons;

namespace Teydes.Service.DTOs.Courses;

public class CourseForUpdateDto : Auditable
{
    public string Name { get; set; }
}
