using Teydes.Service.DTOs.Groups;
using Teydes.Service.DTOs.Users;

namespace Teydes.Service.DTOs.UserGroups;

public class UserGroupForResultDto
{
    public long Id { get; set; }
    public UserForGroupResultDto User { get; set; }
    public GroupForResultDto Group { get; set; }

}
