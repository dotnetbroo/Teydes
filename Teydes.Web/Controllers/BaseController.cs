using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Teydes.Web.Controllers;

[Authorize(Roles = "SuperAdmin, Admin")]
public class BaseController : Controller
{
    public static string NotFoundView { get; } = "ErrorPages/NotFound";
    public static string AccessDeniedView { get; } = "ErrorPages/NotFound";
}
