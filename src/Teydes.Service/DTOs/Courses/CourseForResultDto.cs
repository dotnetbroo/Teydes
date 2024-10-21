using System.Text.Json.Serialization;
using Teydes.Domain.Entities.Courses;
using Teydes.Service.DTOs.Groups;

namespace Teydes.Service.DTOs.Courses;

public class CourseForResultDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public ICollection<GroupForResultDto> Groups { get; set; }
}
