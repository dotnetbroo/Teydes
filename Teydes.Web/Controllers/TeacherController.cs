using AutoMapper;
using Teydes.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Teydes.Service.DTOs.Users;
using Teydes.Domain.Configurations;
using Teydes.Service.DTOs.UserGroups;
using Teydes.Service.Interfaces.Users;
using Teydes.Service.Interfaces.Groups;
using Teydes.Service.Commons.Exceptions;
using Teydes.Service.Interfaces.UserGroups;

namespace Teydes.Web.Controllers;

public class TeacherController : BaseController
{
    private readonly IMapper mapper;
    private readonly IUserService userService;
    private readonly IGroupService groupService;
    private readonly IUserGroupService userGroupService;

    public TeacherController(
        IMapper mapper,
        IUserService userService,
        IGroupService groupService,
        IUserGroupService userGroupService
        )
    {
        this.mapper = mapper;
        this.userService = userService;
        this.groupService = groupService;
        this.userGroupService = userGroupService;
    }

    #region GetAll
    [HttpGet("Teachers")]
    public async Task<ViewResult> Index(string search, int page = 1)
    {
        var paginationParams = new PaginationParams
        {
            PageSize = 30,
            PageIndex = page
        };

        List<UserForResultDto> teachers;

        var courseName = new Dictionary<long, List<string>>();
        if (!string.IsNullOrEmpty(search))
        {
            ViewBag.search = search;
            teachers = (await userService.SearchTeachersAsync(search, paginationParams)).ToList();
            ViewBag.teachers = teachers;


            foreach (var teacher in teachers)
            {
                foreach (var group in teacher.Groups)
                {
                    if (!courseName.ContainsKey(teacher.Id))
                    {
                        courseName[teacher.Id] = new List<string>();
                    }
                    courseName[teacher.Id].Add(group.Name);
                }
            }

            ViewBag.courses = courseName;

            return View("Index");
        }

        teachers = (await userService.RetrieveAllTeachersAsync(paginationParams)).ToList();
        ViewBag.teachers = teachers;

        foreach (var user in teachers)
        {
            foreach (var group in user.Groups)
            {
                if (!courseName.ContainsKey(user.Id))
                {
                    courseName[user.Id] = new List<string>();
                }
                courseName[user.Id].Add(group.Name);
            }
        }

        ViewBag.courses = courseName;

        return View("Index");
    }
    #endregion

    #region Create
    [HttpGet("CreateTeacher")]
    public async Task<ViewResult> CreateTeacherRedirect()
    {
        return View("Create");
    }

    [HttpPost("CreateTeacher")]
    public async Task<IActionResult> CreateTeacherAsync(UserForCreationDto dto)
    {
        try
        {
            if (ModelState.IsValid)
            {
                dto.Role = (UserRole)1;
                var teacher = await userService.AddAsync(dto);
                if (teacher is null)
                {
                    return await CreateTeacherRedirect();
                }
                return RedirectToAction("Index");
            }
            return await CreateTeacherRedirect();
        }
        catch (CustomException ex)
        {
            ModelState.AddModelError(dto.LastName, ex.Message);
            return await CreateTeacherRedirect();
        }
    }
    #endregion

    #region Update
    [HttpGet("UpdateTeacher")]
    public async Task<ViewResult> UpdateRedirectAsync(long teacherId)
    {
        var teacher = await this.userService.RetrieveByIdAsync(teacherId);

        var dto = new UserForUpdateDto()
        {
            FirstName = teacher.FirstName,
            LastName = teacher.LastName,
            PhoneNumber = teacher.PhoneNumber
        };
        
        ViewBag.teacherId = teacherId;

        ViewBag.Groups = this.groupService.GetAll();

        return View("Update", dto);
    }

    [HttpPost("UpdateTeacher")]
    public async Task<IActionResult> UpdateTeacherAsync(UserForUpdateDto dto, long teacherId)
    {
        if (ModelState.IsValid)
        {
            var token = await this.userService.ModifyAsync(teacherId, dto);
            if (token is null)
            {
                return await this.UpdateRedirectAsync(teacherId);
            }
            return RedirectToAction("Index");
        }
        return await this.UpdateRedirectAsync(teacherId);
    }
    #endregion

    #region Remove 
    [HttpGet("DeleteTeacher")]
    public async Task<ViewResult> DeleteRedirectAsync(long id)
    {
        var teacher = await this.userService.RetrieveByIdAsync(id);
        return View("Delete", teacher);
    }

    [HttpPost("DeleteTeacher")]
    public async Task<IActionResult> DeleteTeacherAsync(long id)
    {
        var teacher = await this.userService.RetrieveByIdAsync(id);
        if (teacher is null)
            throw new CustomException(404, "Teacher not found");

        var result = await this.userService.RemoveAsync(id);
        if (result) return RedirectToAction("Index");

        return await DeleteRedirectAsync(id);
    }
    #endregion

    #region Add group
    [HttpPost("AddTeacherGroup")]
    public async Task<IActionResult> AddTeacherGroupAsync(UserGroupForCreationDto dto)
    {
        ViewBag.Groups = this.groupService.GetAll();
        ViewBag.teacherId = dto.UserId;

        var model = await this.userService.RetrieveByIdAsync(dto.UserId);

        var userDto = new UserForUpdateDto()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber
        };

        try
        {
            if (ModelState.IsValid)
            {
                dto.UserId = model.Id;
                var user = await this.userGroupService.AddAsync(dto);
                if (user is null)
                {
                    return View("Update", userDto);
                }
                return View("Update", userDto);
            }
            return View("Update", userDto);
        }
        catch (CustomException ex)
        {
            ModelState.AddModelError(dto.UserId.ToString(), ex.Message);
            return await CreateTeacherRedirect();
        }
    }
        #endregion

    #region Get
    [HttpGet("GetTeacher")]
    public async Task<IActionResult> GetByIdAsync(long teacherId)
    {
        try
        {
            var teacher = await userService.RetrieveByIdAsync(teacherId);
            var teacherViewModel = new UserForResultDto()
            {
                Id = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                PhoneNumber = teacher.PhoneNumber
            };

            return View("Profile", teacherViewModel);
        }
        catch (CustomException exception)
        {
            return View("ErrorPages/NotFound", exception);
        }
    }
    #endregion
}