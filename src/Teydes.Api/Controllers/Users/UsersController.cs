using Teydes.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Teydes.Service.DTOs.Users;
using Teydes.Domain.Configurations;
using Teydes.Api.Controllers.Commons;
using Teydes.Service.Interfaces.Users;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace Teydes.Api.Controllers.Users;

[Authorize]
public class UsersController : BaseController
{
    private readonly IUserService userService;
    public UsersController(IUserService userService)
    {
        this.userService = userService;
    }

    /// <summary>
    /// Create new users
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Authorize(Policy = "Admins")]
    [HttpPost]
    public async Task<ActionResult<UserForResultDto>> PostAsync([FromBody] UserForCreationDto dto)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await userService.AddAsync(dto)
        });

    /// <summary>
    /// Get all users
    /// </summary>
    /// <param name="params"></param>
    /// <returns></returns>
    [Authorize(Policy = "TeachersAndAdmins")]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await userService.RetrieveAllAsync(@params)
        });

    /// <summary>
    /// Get by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(Policy = "TeachersAndAdmins")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute(Name = "id")] long id)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await userService.RetrieveByIdAsync(id)
        });

    [Authorize(Policy = "TeachersAndAdmins")]
    [HttpGet("retrieve-with-last-quiz{id}")]
    public async Task<IActionResult> GetWithLastQuizzesAsync([FromRoute(Name = "id")] long id)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await userService.RetrieveByIdWithQuizzesAsync(id)
        });


    /// <summary>
    /// Update users info
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Authorize(Policy = "Admins")]
    [HttpPut("{id}")]
    public async Task<ActionResult<UserForResultDto>> PutAsync([FromRoute(Name = "id")] long id, [FromBody] UserForUpdateDto dto)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await userService.ModifyAsync(id, dto)
        });

    /// <summary>
    /// Delete by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(Policy = "Admins")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteAsync([FromRoute(Name = "id")] long id)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await userService.RemoveAsync(id)
        });

    /// <summary>
    /// Change user password
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    
    [HttpPut("change-password")]
    public async Task<ActionResult<UserForResultDto>> ChangePasswordAsync(long id, UserForChangePasswordDto dto)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await userService.ChangePasswordAsync(id, dto)
        });

    [AllowAnonymous]
    [HttpPut("forget-password")]
    public async Task<IActionResult> ForgetPasswordAsync([Required] string PhoneNumber, [Required] string NewPassword, [Required] string ConfirmPassword)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await userService.ForgetPasswordAsync(PhoneNumber, NewPassword, ConfirmPassword)
        });


    [AllowAnonymous]
    [HttpGet("phone-number")]
    public async Task<IActionResult> RetrievePhoneNumberAsync(string phoneNumber)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await this.userService.RetrieveByPhoneNumberAsync(phoneNumber)
        });

    [Authorize(Policy = "TeachersAndAdmins")]
    [HttpPost("send-message-to-user")]
    public async Task<IActionResult> SendMessageByTelegramUser(long telegramId, string text, string url)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await this.userService.SendMessageByTelegramIdAsync(telegramId, text, url)
        });

    [AllowAnonymous]
    [HttpGet("CheckUser/{telegram-id}")]
    public async Task<IActionResult> TelegramIdExistsCheckAsync([FromRoute(Name = "telegram-id")] long telegramId)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await this.userService.TelegramIdExistsAsync(telegramId)
        });
    [AllowAnonymous]
    [HttpPatch("Users/{id}/{telegramId}")]
    public async Task<IActionResult> TelegramIdUpdateAsync([FromRoute(Name = "id")] long id, [FromRoute(Name = "telegramId")] long telegramId)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await this.userService.ModifyTelegramId(id, telegramId)
        });

    [Authorize(Policy = "Admins")]
    [HttpGet("retrieve-all-teachers")]
    public async Task<IActionResult> GetAllTeachersAsync([FromQuery] PaginationParams @params)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await this.userService.RetrieveAllTeachersAsync(@params)
        });

    [Authorize(Policy = "TeachersAndAdmins")]
    [HttpGet("users-statistcs")]
    public async Task<IActionResult> GetAllWithStatisticsAsync([FromQuery] PaginationParams @params)
       => Ok(new Response
       {
           Code = 200,
           Message = "Success",
           Data = await userService.RetrieveAllStudentsWithStatisticsAsync(@params)
       });

}
