using Teydes.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Teydes.Service.DTOs.Logins;
using Teydes.Service.Interfaces.Accounts;
using Teydes.Service.Interfaces.Commons;

namespace Teydes.Api.Controllers.Commons;

public class AuthController : BaseController
{
    private readonly IAccountService accountService;

    public AuthController(IAccountService accountService, IAuthService authService)
    {
        this.accountService = accountService;
    }

    [HttpPost]
    [Route("login")]
    public async ValueTask<IActionResult> login([FromBody] LoginDto loginDto)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await accountService.LoginAsync(loginDto)
        });
}
