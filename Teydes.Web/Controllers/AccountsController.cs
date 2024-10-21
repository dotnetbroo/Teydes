using Microsoft.AspNetCore.Mvc;
using Teydes.Service.DTOs.Users;
using Teydes.Service.DTOs.Logins;
using Teydes.Service.Commons.Helpers;
using Teydes.Service.Interfaces.Users;
using Teydes.Service.Commons.Exceptions;
using Teydes.Service.Interfaces.Accounts;
using System.Net;

[Route("accounts")]
public class AccountsController : Controller
{
    private readonly IUserService userService;
    private readonly IAccountService accountService;

    public AccountsController(IAccountService accountService, IUserService userService)
    {
        this.userService = userService;
        this.accountService = accountService;
    }
    #region Index
    public IActionResult Index()
    {
        return RedirectToAction("index", "admins");//View();
    }
    #endregion

    #region Login 
    [HttpGet("login")]
    public IActionResult Login() => View("login");

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto loginDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string token = await accountService.LoginAsync(loginDto);

                    Response.Cookies.Append("X-Access-Token", token, new CookieOptions
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = TimeHelper.GetCurrentServerTime().AddMinutes(1)
                    });
                return RedirectToAction("index", "admins");
                }
                else return View();
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
    #endregion

    #region Logout
    [HttpGet("logout")]
    public IActionResult LogOut()
    {
        HttpContext.Response.Cookies.Append("X-Access-Token", "", new CookieOptions
        {
            Expires = TimeHelper.GetCurrentServerTime().AddDays(-1)
        });
        return RedirectToAction("login", "accounts", new { area = "" });
    }
    #endregion
}
