using Teydes.Domain.Commons;

namespace Teydes.Domain.Entities.Courses;

public class Course : Auditable
{
    public string Name { get; set; }

    public ICollection<Group> Groups { get; set; }
}
