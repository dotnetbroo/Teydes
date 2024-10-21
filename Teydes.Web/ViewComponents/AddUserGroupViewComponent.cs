using Microsoft.AspNetCore.Mvc;
using Teydes.Service.DTOs.UserGroups;
using Teydes.Service.Interfaces.Groups;
using Teydes.Service.Interfaces.UserGroups;

namespace Teydes.Web.ViewComponents;

public class AddUserGroupViewComponent : ViewComponent
{
   /* private readonly IUserGroupService _userGroupService;

    public AddUserGroupViewComponent(IUserGroupService userGroupService)
    {
        _userGroupService = userGroupService;
    }*/

    public IViewComponentResult Invoke()
    {
        UserGroupForUpdateDto changeGroupDto = new UserGroupForUpdateDto();
        return View(changeGroupDto);
    }
}
