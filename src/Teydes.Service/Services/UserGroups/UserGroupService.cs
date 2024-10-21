using AutoMapper;
using Teydes.Data.IRepositories;
using Teydes.Domain.Configurations;
using Microsoft.EntityFrameworkCore;
using Teydes.Domain.Entities.Courses;
using Teydes.Service.Commons.Helpers;
using Teydes.Service.DTOs.UserGroups;
using Teydes.Service.Interfaces.Users;
using Teydes.Service.Interfaces.Groups;
using Teydes.Service.Commons.Exceptions;
using Teydes.Service.Commons.Extensions;
using Teydes.Service.Interfaces.UserGroups;

namespace Teydes.Service.Services.UserGroups;

public class UserGroupService : IUserGroupService
{
    private readonly IMapper mapper;
    private readonly IUserService userService;
    private readonly IGroupService groupService;
    private readonly IRepository<UserGroup> userGroupRepository;

    public UserGroupService(
        IMapper mapper,
        IUserService userService,
        IGroupService groupService,
        IRepository<UserGroup> userGroupRepository)
    {
        this.mapper = mapper;
        this.userService = userService;
        this.groupService = groupService;
        this.userGroupRepository = userGroupRepository;
    }

    public async Task<UserGroupForResultDto> AddAsync(UserGroupForCreationDto dto)
    {
        var group = await this.groupService.RetrieveByIdAsync(dto.GroupId);
        var user = await this.userService.RetrieveByIdAsync(dto.UserId);

        var userGroup = this.mapper.Map<UserGroup>(dto);
        userGroup.CreatedAt = TimeHelper.GetCurrentServerTime();

        var result = await this.userGroupRepository.InsertAsync(userGroup);
        await this.userGroupRepository.SaveAsync();

        return this.mapper.Map<UserGroupForResultDto>(result);
    }

    public IEnumerable<UserGroupForResultDto> GetAll()
    {
        var userGroups = this.userGroupRepository.SelectAll()
            .AsNoTracking()
            .ToList();

        return this.mapper.Map<IEnumerable<UserGroupForResultDto>>(userGroups);
    }

    public async Task<UserGroupForResultDto> ModifyAsync(long id, UserGroupForUpdateDto dto)
    {
        var userGroup = await this.userGroupRepository.SelectAsync(u => u.Id == id);
        if (userGroup is null)
            throw new CustomException(404, "UserGroup not found");

        var user = await this.userService.RetrieveByIdAsync(dto.UserId);
        var group = await this.groupService.RetrieveByIdAsync(dto.GroupId);

        userGroup.UpdatedAt = TimeHelper.GetCurrentServerTime();
        var mappedUserGroup = this.mapper.Map(dto, userGroup);
        await this.userGroupRepository.SaveAsync();

        return this.mapper.Map<UserGroupForResultDto>(mappedUserGroup);
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var userGroup = await this.userGroupRepository.SelectAsync(u => u.Id == id);
        if (userGroup is null)
            throw new CustomException(404, "UserGroup not found");

        await userGroupRepository.DeleteAsync(id);
        var result = await this.userGroupRepository.SaveAsync();

        return result;
    }

    public async Task<IEnumerable<UserGroupForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var userGroups = await this.userGroupRepository.SelectAll()
            .Include(ug => ug.Group)
            .Include(ug => ug.User)
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync();

        return this.mapper.Map<IEnumerable<UserGroupForResultDto>>(userGroups);
    }

    public async Task<UserGroupForResultDto> RetrieveByIdAsync(long id)
    {
        var userGroup = await this.userGroupRepository.SelectAll()
            .Where(u => u.Id == id)
            .Include(ug => ug.Group)
            .Include(ug => ug.User)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        if (userGroup is null)
            throw new CustomException(404, "Group not found");

        return this.mapper.Map<UserGroupForResultDto>(userGroup);
    }
}
