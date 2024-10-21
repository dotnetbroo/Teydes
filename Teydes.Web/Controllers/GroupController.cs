using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Teydes.Service.DTOs.Groups;
using Teydes.Domain.Configurations;
using Teydes.Service.Interfaces.Groups;
using Teydes.Service.Commons.Exceptions;
using Teydes.Service.Interfaces.Courses;
using Teydes.Service.DTOs.Courses;
using Teydes.Service.DTOs.Users;
using Teydes.Service.Services.Users;
using Teydes.Service.Services.UserGroups;
using Teydes.Service.Interfaces.UserGroups;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Teydes.Service.Interfaces.Users;

namespace Teydes.Web.Controllers;

public class GroupController : BaseController
{
    private readonly IMapper mapper;
    private readonly IUserService userService;
    private readonly IGroupService groupService;
    private readonly ICourseService courseService;
    private readonly IUserGroupService userGroupService;

    public GroupController
        (
        IMapper mapper,
        IUserService userService,
        IGroupService groupService,
        ICourseService courseService,
        IUserGroupService userGroupService
        )
    {
        this.mapper = mapper;
        this.userService = userService;
        this.groupService = groupService;
        this.courseService = courseService;
        this.userGroupService = userGroupService;
    }

    #region GetAll
    [HttpGet("Groups")]
    public async Task<ViewResult> Index(string search, int page = 1)
    {
        var paginationParams = new PaginationParams
        {
            PageSize = 30,
            PageIndex = page
        };

        List<GroupForResultDto> groups;
        var courseName = new Dictionary<long, string>();

        if (!string.IsNullOrEmpty(search))
        {
            ViewBag.search = search;

            groups = (await groupService.SearchAllAsync(search, paginationParams)).ToList();
            ViewBag.groups = groups;

            var course = (await courseService.RetrieveAllAsync(paginationParams)).ToList();
            ViewBag.courses = course;

            return View("Index");
        }

        groups = (await groupService.RetrieveAllAsync(paginationParams)).ToList();
        ViewBag.groups = groups;

        var course1 = (await courseService.RetrieveAllAsync(paginationParams)).ToList();
        ViewBag.courses = course1;

        return View("Index");
    }
    #endregion

    #region Create
    [HttpGet("CreateGroup")]
    public async Task<ViewResult> CreateGroupRedirect()
    {
        ViewBag.Courses = await this.courseService.GetAllAsync();
        return View("Create");
    }

    [HttpPost("CreateGroup")]
    public async Task<IActionResult> CreateGroupAsync(GroupForCreationDto dto)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var group = await groupService.AddAsync(dto);
                if (group is null)
                {
                    return await CreateGroupRedirect();
                }
                return RedirectToAction("Index");
            }
            return await CreateGroupRedirect();
        }
        catch (CustomException ex)
        {
            ModelState.AddModelError(dto.Name, ex.Message);
            return await CreateGroupRedirect();
        }
    }
    #endregion

    #region Update
    [HttpGet("UpdateGroup")]
    public async Task<ViewResult> UpdateRedirectAsync(long groupId)
    {
        var group = await this.groupService.RetrieveByIdAsync(groupId);

        var dto = new GroupForUpdateDto()
        {
            Id = groupId,
            Name = group.Name,
            Contain = group.Contain,
            CourseId = group.CourseId,
        };

        ViewBag.Id = group.Id;
        ViewBag.Courses = await this.courseService.GetAllAsync();

        return View("Update", dto);
    }

    [HttpPost("UpdateGroup")]
    public async Task<IActionResult> UpdateGroupAsync(GroupForUpdateDto dto, long groupId)
    {
        if (ModelState.IsValid)
        {
            var model = await this.groupService.ModifyAsync(groupId, dto);
            if (model is null)
            {
                return await this.UpdateRedirectAsync(groupId);
            }
            return RedirectToAction("Index");
        }
        return await this.UpdateRedirectAsync(groupId);
    }
    #endregion

    #region Remove 
    [HttpGet("DeleteGroup")]
    public async Task<ViewResult> DeleteRedirectAsync(long id)
    {
        var group = await this.groupService.RetrieveByIdAsync(id);

        var course = await this.courseService.RetrieveByIdAsync(group.CourseId);

        ViewBag.CourseName = course.Name;

        return View("Delete", group);
    }

    [HttpPost("DeleteGroup")]
    public async Task<IActionResult> DeleteGroupAsync(long id)
    {
        var group = await this.groupService.RetrieveByIdAsync(id);
        if (group is null)
            throw new CustomException(404, "Group not found");

        var result = await this.groupService.RemoveAsync(id);
        if (result) return RedirectToAction("Index");

        return await DeleteRedirectAsync(id);
    }
    #endregion

    #region Get
    [HttpGet("GetGroup")]
    public async Task<IActionResult> GetByIdAsync(long groupId)
    {
        try
        {
            var group = await groupService.RetrieveByIdAsync(groupId);
            var course = await courseService.RetrieveByIdAsync(group.CourseId);
            var groupViewModel = new GroupForResultDto()
            {
                Id = group.Id,
                Name = group.Name,
                Contain = group.Contain,
            };

            ViewBag.Course = course.Name;

            ViewBag.Users = await userService.GetAllByGroupAsync(groupId);
                
            return View("Profile", groupViewModel);
        }
        catch (CustomException exception)
        {
            return View("ErrorPages/NotFound", exception);
        }
    }
    #endregion

   /* #region Drop user
    [HttpPost("DropUserInGroup")]
    public async Task<IActionResult> DropUserInGroupAsync(long userId, long groupId)
    {
        var teacher = await this.userService.RetrieveByIdAsync(id);
        if (teacher is null)
            throw new CustomException(404, "Teacher not found");

        var result = await this.userService.RemoveAsync(id);
        if (result) return RedirectToAction("Index");

        return await DeleteRedirectAsync(id);
    }
    #endregion */
}
