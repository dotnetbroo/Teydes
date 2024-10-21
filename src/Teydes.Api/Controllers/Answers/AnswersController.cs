using Teydes.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Teydes.Service.DTOs.Answers;
using Teydes.Domain.Configurations;
using Teydes.Api.Controllers.Commons;
using Teydes.Service.Interfaces.Answers;
using Microsoft.AspNetCore.Authorization;

namespace Teydes.Api.Controllers.Answers;

[Authorize]
public class AnswersController : BaseController
{
    private readonly IQuestionAnswerService answerService;

    public AnswersController(IQuestionAnswerService answerService)
    {
        this.answerService = answerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await answerService.RetrieveAllAsync(@params)
        });

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute(Name = "id")] long id)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await answerService.RetrieveByIdAsync(id)
        });

    [Authorize(Policy = "TeachersAndAdmins")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await answerService.RemoveAsync(id)
        });
    [Authorize(Policy = "TeachersAndAdmins")]
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] QuestionAnswerForCreationDto dto)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await answerService.CreateAnswer(dto)
        });
    [Authorize(Policy = "TeachersAndAdmins")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PostAsync([FromRoute(Name = "id")] long id, [FromBody] QuestionAnswerForUpdateDto dto)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await answerService.ModifyAnswer(id, dto)
        });

    [HttpGet("questionId")]
    public async Task<IActionResult> GetByQuestionIdAsync(long questionId)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await this.answerService.RetrieveByQuestionIdAsync(questionId)
        }
        );

}
