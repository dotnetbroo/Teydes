using Microsoft.AspNetCore.Mvc;
using Teydes.Service.DTOs.Users;
using Teydes.Service.Interfaces.Users;

namespace Teydes.Web.ViewComponents;

public class AdminPasswordUpdateViewComponent : ViewComponent
{
    private readonly IUserService _userService;

    public AdminPasswordUpdateViewComponent(IUserService userService)
    {
        _userService = userService;
    }

    public IViewComponentResult Invoke()
    {
        UserForChangePasswordDto changePasswordDto = new UserForChangePasswordDto();
        return View(changePasswordDto);
    }
}
