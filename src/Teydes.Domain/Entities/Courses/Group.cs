using Teydes.Domain.Commons;
using Teydes.Domain.Entities.Quizes;

namespace Teydes.Domain.Entities.Courses;

public class Group : Auditable
{
    public string Name { get; set; }
    public int Contain { get; set; }
    public long CourseId { get; set; }
    public Course Course { get; set; }

    public ICollection<Quiz> Quizzes { get; set; }
    public ICollection<UserGroup> UserGroups { get; set; }
}
    