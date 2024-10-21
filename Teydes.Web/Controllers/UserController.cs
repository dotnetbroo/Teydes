using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Teydes.Domain.Configurations;
using Teydes.Domain.Enums;
using Teydes.Service.Commons.Exceptions;
using Teydes.Service.DTOs.UserGroups;
using Teydes.Service.DTOs.Users;
using Teydes.Service.Interfaces.Users;
using Teydes.Service.Interfaces.Groups;
using Teydes.Service.Interfaces.UserGroups;

namespace Teydes.Web.Controllers;
public class UserController : BaseController
{
    private readonly IMapper mapper;
    private readonly IUserService userService;
    private readonly IGroupService groupService;
    private readonly IUserGroupService userGroupService;

    public UserController(
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
    [HttpGet("Users")]
    public async Task<ViewResult> Index(string search, int page = 1)
    {
        var paginationParams = new PaginationParams
        {
            PageSize = 30,
            PageIndex = page
        };

        List<UserForResultDto> users;
        //var courseName = new Dictionary<long, string>();
        var courseName = new Dictionary<long, List<string>>();
        if (!string.IsNullOrEmpty(search))
        {
            ViewBag.search = search;
            users = (await userService.SearchAllAsync(search, paginationParams)).ToList();
            ViewBag.users = users;


            foreach (var user in users)
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

        users = (await userService.RetrieveAllAsync(paginationParams)).ToList();
        ViewBag.users = users;

        foreach (var user in users)
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
    [HttpGet("CreateUser")]
    public async Task<ViewResult> CreateUserRedirect()
    {
        return View("Create");
    }

    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUserAsync(UserForCreationDto dto)
    {
        try
        {
            if (ModelState.IsValid)
            {
                dto.Role = (UserRole)0;
                var user = await userService.AddAsync(dto);
                if (user is null)
                {
                    return await CreateUserRedirect();
                }
                return RedirectToAction("Index");
            }
            return await CreateUserRedirect();
        }
        catch (CustomException ex)
        {
            ModelState.AddModelError(dto.LastName, ex.Message);
            return await CreateUserRedirect();
        }
    }
    #endregion

    #region Update
    [HttpGet("UpdateUser")]
    public async Task<ViewResult> UpdateRedirectAsync(long userId)
    {
        var user = await this.userService.RetrieveByIdAsync(userId);

        var dto = new UserForUpdateDto()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            IsStudyForeign = user.IsStudyForeign,
        };

        ViewBag.Id = user.Id;

        ViewBag.Groups = this.groupService.GetAll();

        return View("Update", dto);
    }

    [HttpPost("UpdateUser")]
    public async Task<IActionResult> UpdateUserAsync(UserForUpdateDto dto, long userId)
    {
        if (ModelState.IsValid)
        {
            var token = await this.userService.ModifyAsync(userId, dto);
            if (token is null)
            {
                return await this.UpdateRedirectAsync(userId);
            }
            return RedirectToAction("Index");
        }
        return await this.UpdateRedirectAsync(userId);
    }
    #endregion

    #region Remove 
    [HttpGet("DeleteUser")]
    public async Task<ViewResult> DeleteRedirectAsync(long id)
    {
        var user = await this.userService.RetrieveByIdAsync(id);
        return View("Delete", user);
    }

    [HttpPost("DeleteUser")]
    public async Task<IActionResult> DeleteUserAsync(long id)
    {
        var user = await this.userService.RetrieveByIdAsync(id);
        if (user is null)
            throw new CustomException(404, "User not found");

        var result = await this.userService.RemoveAsync(id);
        if (result) return RedirectToAction("Index");

        return await DeleteRedirectAsync(id);
    }
    #endregion

    #region Add group
    [HttpPost("AddUserGroup")]
    public async Task<IActionResult> AddUserGroupAsync(UserGroupForCreationDto dto)
    {
        ViewBag.Groups = this.groupService.GetAll();

        ViewBag.Id = dto.UserId;

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
                dto.UserId = ViewBag.Id;
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
            return await CreateUserRedirect();
        }
    }
    #endregion

    #region Get
    [HttpGet("GetUser")]
    public async Task<IActionResult> GetByIdAsync(long userId)
    {
        try
        {
            var user = await userService.RetrieveByIdAsync(userId);
            var userViewModel = new UserForResultDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                IsStudyForeign = user.IsStudyForeign,
                

            };

            return View("Profile", userViewModel);
        }
        catch (CustomException exception)
        {
            return View("ErrorPages/NotFound", exception);
        }
    }
    #endregion

}
