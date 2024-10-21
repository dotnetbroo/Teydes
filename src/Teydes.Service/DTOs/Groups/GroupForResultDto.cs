using Teydes.Domain.Entities.Quizes;
using Teydes.Domain.Entities.Courses;
using Teydes.Service.DTOs.Courses;
using System.Text.Json.Serialization;

namespace Teydes.Service.DTOs.Groups;

public class GroupForResultDto
{
    public long Id { get; set; }
    public int Contain { get; set; }
    public string Name { get; set; }
    public long CourseId { get; set; }
}