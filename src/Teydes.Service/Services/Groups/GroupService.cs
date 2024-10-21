using AutoMapper;
using Teydes.Data.IRepositories;
using Teydes.Service.DTOs.Groups;
using Teydes.Domain.Configurations;
using Microsoft.EntityFrameworkCore;
using Teydes.Service.Commons.Helpers;
using Teydes.Domain.Entities.Courses;
using Teydes.Service.Interfaces.Groups;
using Teydes.Service.Commons.Extensions;
using Teydes.Service.Commons.Exceptions;
using Teydes.Service.Interfaces.Courses;
using Teydes.Service.Interfaces.Users;
using Teydes.Service.DTOs.Users;

namespace Teydes.Service.Services.Groups;

public class GroupService : IGroupService
{
    private readonly IMapper mapper;
    private readonly IUserService userService;
    private readonly ICourseService courseService;
    private readonly IRepository<Group> groupRepository;

    public GroupService(
        IMapper mapper,
        ICourseService courseService,
        IRepository<Group> groupRepository,
        IUserService userService)
    {
        this.mapper = mapper;
        this.courseService = courseService;
        this.groupRepository = groupRepository;
        this.userService = userService;
    }
    public async Task<GroupForResultDto> AddAsync(GroupForCreationDto dto)
    {
        var group = await this.groupRepository.SelectAsync(u => u.Name == dto.Name);
        if (group is not null)
            throw new CustomException(403, "Group is already exists");

        var courseCheck = await this.courseService.RetrieveByIdAsync(dto.CourseId);
        if (courseCheck is null)
            throw new CustomException(400, "Course not found");

        var forCreation = this.mapper.Map<Group>(dto);
        forCreation.CreatedAt = TimeHelper.GetCurrentServerTime(); ;

        var result = await this.groupRepository.InsertAsync(forCreation);
        await this.groupRepository.SaveAsync();

        return this.mapper.Map<GroupForResultDto>(result);
    }

    public IEnumerable<GroupForResultDto> GetAll()
    {
        var groups = this.groupRepository.SelectAll()
            .AsNoTracking();

        return this.mapper.Map<IEnumerable<GroupForResultDto>>(groups);
    }

    public async Task<GroupForResultDto> ModifyAsync(long id, GroupForUpdateDto dto)
    {
        var group = await this.groupRepository.SelectAsync(u => u.Id == id);
        if (group is null)
            throw new CustomException(404, "Group not found");

        if (dto is not null)
        {
            group.Name = string.IsNullOrEmpty(dto.Name) ? group.Name : dto.Name;
            group.Contain = dto.Contain;
            group.CourseId = dto.CourseId;

            group.UpdatedAt = TimeHelper.GetCurrentServerTime();

            this.groupRepository.Update(group);
            await this.groupRepository.SaveAsync();
        }

        var @class = this.mapper.Map(dto, group);

        return this.mapper.Map<GroupForResultDto>(@class);
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var group = await this.groupRepository.SelectAsync(u => u.Id == id);
        if (group is null)
            throw new CustomException(404, "Group not found");

        await groupRepository.DeleteAsync(id);
        var result = await this.groupRepository.SaveAsync();
        return result;
    }

    public async Task<IEnumerable<GroupForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var groups = await this.groupRepository.SelectAll()
            .Include(c => c.Course)
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync();

        return this.mapper.Map<IEnumerable<GroupForResultDto>>(groups);
    }

    public async Task<GroupForResultDto> RetrieveByIdAsync(long id)
    {
        var group = await this.groupRepository.SelectAll()
            .Where(u => u.Id == id)
            .Include(c => c.Course)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        if (group is null)
            throw new CustomException(404, "Group not found");

        return this.mapper.Map<GroupForResultDto>(group);
    }

    public async Task<GroupForStudentRankingResultDto> RetrieveByIdWithStudentRankingAsync(long id)
    {
        var group = await this.groupRepository.SelectAll()
        .Where(g => g.Id == id)
        .Include(g => g.UserGroups)
        .ThenInclude(ug => ug.User)
        .ThenInclude(u => u.QuizResults)  // Include QuizResults to be able to use it in the ordering
        .AsNoTracking()
        .FirstOrDefaultAsync();

        if (group is null)
            throw new CustomException(404, "Group is not found");

        // Order users by their quiz score
        var orderedUsers = group?.UserGroups
            .OrderByDescending(ug => ug.User?.QuizResults?.Average(qr => qr.Score))
            .Select(ug => ug.User)
            .ToList();

        var mappedGroup = this.mapper.Map<GroupForStudentRankingResultDto>(group);

        var mappedUsers = this.mapper.Map<IEnumerable<UserStatisticsForResultDto>>(orderedUsers);

        var users = await this.userService.RetrieveAllStudentsWithStatisticsAsync();
        foreach (var user in mappedUsers)
        {
            var person = users.Where(u => u.Id == user.Id).FirstOrDefault();
            user.TestCount = person.TestCount;
            user.TotalScore = person.TotalScore;
        }

        mappedGroup.Users = mappedUsers.ToList();

        return mappedGroup;
    }

    public async Task<IEnumerable<GroupForResultDto>> SearchAllAsync(string search, PaginationParams @params)
    {
        var groups = await this.groupRepository.SelectAll()
            .Where(x => x.Name.ToLower().Contains(search.ToLower()))
           .Include(c => c.Course)
           .AsNoTracking()
           .ToPagedList(@params)
           .ToListAsync();

        return this.mapper.Map<IEnumerable<GroupForResultDto>>(groups);
    }
}
