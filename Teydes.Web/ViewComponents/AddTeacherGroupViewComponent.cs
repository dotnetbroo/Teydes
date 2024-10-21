using Microsoft.AspNetCore.Mvc;
using Teydes.Service.DTOs.UserGroups;
using Teydes.Service.Interfaces.UserGroups;

namespace Teydes.Web.ViewComponents;

public class AddTeacherGroupViewComponent : ViewComponent
{
    //private readonly IUserGroupService _userGroupService;

    //public AddTeacherGroupViewComponent(IUserGroupService userGroupService)
    //{
    //    _userGroupService = userGroupService;
    //}

    public IViewComponentResult Invoke()
    {
        UserGroupForUpdateDto changeGroupDto = new UserGroupForUpdateDto();
        return View(changeGroupDto);
    }
}
