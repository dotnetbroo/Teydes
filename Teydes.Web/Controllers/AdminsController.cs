using AutoMapper;
using Teydes.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Teydes.Service.DTOs.Users;
using Teydes.Domain.Configurations;
using Teydes.Service.Commons.Helpers;
using Teydes.Service.Interfaces.Users;
using Teydes.Service.Commons.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace Teydes.Web.Controllers;


public class AdminsController : BaseController
{
    private readonly IMapper mapper;
    private readonly IUserService userService;
    public AdminsController(IUserService userService, IMapper mapper)
    {
        this.mapper = mapper;
        this.userService = userService;
    }

    #region GetAll
    [HttpGet("Admins")]
    public async Task<ViewResult> Index(string search, int page = 1)
    {
        var paginationParams = new PaginationParams
        {
            PageSize = 30,
            PageIndex = page
        };

        List<UserForResultDto> admins;

        if (!string.IsNullOrEmpty(search))
        {
            ViewBag.search = search;
            admins = (await userService.SearchAdminsAsync(search, paginationParams)).ToList();
            ViewBag.admins = admins;
            return View("Index");
        }

        admins = (await userService.RetrieveAllAdminsAsync(paginationParams)).ToList();
        ViewBag.admins = admins;
        return View("Index");
    }
    #endregion

    #region Create
    [HttpGet("CreateAdmin")]
    public ViewResult CreateAdminRedirect()
    {
        return View("Create");
    }

    [HttpPost("CreateAdmin")]
    public async Task<IActionResult> CreateAdminAsync(UserForCreationDto adminDto)
    {
        var role = HttpContextHelper.UserRole.ToString();
        if (role == "SuperAdmin")
        {
            try
            {
                if (ModelState.IsValid)
                {
                    adminDto.Role = (UserRole)2;
                    var admin = await userService.AddAsync(adminDto);
                    if (admin is null)
                    {
                        return CreateAdminRedirect();
                    }
                    return RedirectToAction("Index");// View("Index");//RedirectToAction("Admins");
                }
                return CreateAdminRedirect();
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError(adminDto.LastName, ex.Message);
                return CreateAdminRedirect();
            }
        }
        return Forbid(); // проверять на роль, если не проходит аутентификацую то возврашает "403 Forbidden", что означает  у него нет прав доступа!!!
    }
    #endregion

    #region Update
    [HttpGet("UpdateAdmin")]
    public async Task<ViewResult> UpdateRedirectAsync(long adminId)
    {
        var admin = await this.userService.RetrieveByIdAsync(adminId);

        var dto = new UserForUpdateDto()
        {
            FirstName = admin.FirstName,
            LastName = admin.LastName,
            PhoneNumber = admin.PhoneNumber
        };

        ViewBag.Id = admin.Id;
        ViewBag.AdminRole = admin.Role;

        return View("Update", dto);
    }

    [HttpPost("UpdateAdmin")]
    public async Task<IActionResult> UpdateAdminAsync(UserForUpdateDto dto, long adminId)
    {
        if (ModelState.IsValid)
        {
            var token = await this.userService.ModifyAsync(adminId, dto);
            if (token is null)
            {
                return await this.UpdateRedirectAsync(adminId);
            }
            return RedirectToAction("Index");
            //  else return RedirectToAction("Update", "Admins");
        }
        return await this.UpdateRedirectAsync(adminId);
    }
    #endregion

    #region Change-Password

    [HttpPost("PasswordUpdateAdmin")]
    public async Task<IActionResult> PasswordUpdateAsync(long id, UserForChangePasswordDto dto)
    {
        ViewBag.Id = id;
        var admin = await this.userService.RetrieveByIdAsync(id);

        var adminDto = new UserForUpdateDto()
        {
            FirstName = admin.FirstName,
            LastName = admin.LastName,
            PhoneNumber = admin.PhoneNumber
        };

        try
        {
            if (ModelState.IsValid == false) return View("Update", adminDto);

            var result = await userService.ChangePasswordAsync(id, dto);
            if (result) return View("Update", adminDto);

            return View("Update", adminDto);
        }
        catch (CustomException ex)
        {
            ModelState.AddModelError(adminDto.LastName, ex.Message);
            return CreateAdminRedirect();
        }
    }
    #endregion

    #region Remove 
    [HttpGet("DeleteAdmin")]
    public async Task<ViewResult> DeleteRedirectAsync(long id)
    {
        var admin = await this.userService.RetrieveByIdAsync(id);
        return View("Delete", admin);
    }

    [HttpPost("DeleteAdmin")]
    public async Task<IActionResult> DeleteAdminAsync(long id)
    {
        var admin = await this.userService.RetrieveByIdAsync(id);
        if (admin is null)
            throw new CustomException(404, "Admin not found");

        var result = await this.userService.RemoveAsync(id);
        if (result) return RedirectToAction("Index");

        return await DeleteRedirectAsync(id);
    }
    #endregion

    #region Get
    [HttpGet("GetAdmin")]
    public async Task<IActionResult> GetByIdAsync(long adminId)
    {
        try
        {
            var admin = await userService.RetrieveByIdAsync(adminId);
            var adminViewModel = new UserForResultDto()
            {
                Id = admin.Id,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                PhoneNumber = admin.PhoneNumber,
                Role = admin.Role
            };

            return View("Profile", adminViewModel);
        }
        catch (CustomException exception)
        {
            return View("ErrorPages/NotFound", exception);
        }
    }
    #endregion
}
