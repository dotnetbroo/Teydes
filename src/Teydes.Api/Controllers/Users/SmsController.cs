using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teydes.Api.Controllers.Commons;
using Teydes.Service.DTOs;
using Teydes.Service.Interfaces.Users;
using Teydes.Service.Services.Users;
using Teydes.Shared.Models;

namespace Teydes.Api.Controllers.Users;

[Authorize]
public class SmsController : BaseController
{
    private readonly ISmsService smsService;

    public SmsController(ISmsService smsService)
    {
        this.smsService = smsService;
    }

    [Authorize(Policy = "TeachersAndAdmins")]
    [HttpPost]
    public async Task<IActionResult> SendMessageAsync(Message message)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await this.smsService.SendAsync(message)
        });



    [Authorize(Policy = "TeachersAndAdmins")]
    [HttpPost("send-message-by-telegram/groupId/url/text")]
    public async Task<IActionResult> PostAsync(long groupId, string url, string text)
    {
        // Decode the URL parameter
        url = Uri.UnescapeDataString(url);

        return Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await smsService.SendMessageByTelegramAsync(groupId, url, text)
        });
    }
}

