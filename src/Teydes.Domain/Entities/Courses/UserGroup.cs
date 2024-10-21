using Teydes.Domain.Commons;
using Teydes.Domain.Entities.Users;

namespace Teydes.Domain.Entities.Courses;

public class UserGroup : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; }
    public long GroupId { get; set; }
    public Group Group { get; set; }

}
