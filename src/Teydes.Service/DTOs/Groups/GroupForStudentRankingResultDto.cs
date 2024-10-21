using Teydes.Service.DTOs.Users;

namespace Teydes.Service.DTOs.Groups;

public class GroupForStudentRankingResultDto
{
    public string Name { get; set; }
    public ICollection<UserStatisticsForResultDto> Users { get; set; }
}
