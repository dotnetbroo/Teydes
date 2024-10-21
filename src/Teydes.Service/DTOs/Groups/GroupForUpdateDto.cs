using Teydes.Domain.Commons;

namespace Teydes.Service.DTOs.Groups;

public class GroupForUpdateDto : Auditable
{
    public string Name { get; set; }
    public int Contain { get; set; }
    public long CourseId { get; set; }
}
