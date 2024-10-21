using Teydes.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Teydes.Domain.Configurations;
using Teydes.Service.DTOs.Questions;
using Teydes.Api.Controllers.Commons;
using Teydes.Service.Interfaces.Questions;
using Microsoft.AspNetCore.Authorization;

namespace Teydes.Api.Controllers.Questions;

[Authorize]
public class QuestionsController : BaseController
{
    private readonly IQuestionService questionService;
    public QuestionsController(IQuestionService questionService)
    {
        this.questionService = questionService;
    }

    [Authorize(Policy = "TeachersAndAdmins")]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await this.questionService.RetrieveAllAsync(@params)
        });

    [Authorize(Policy = "TeachersAndAdmins")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute(Name = "id")] long id)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await this.questionService.RetrieveAsync(id)
        });

    [Authorize(Policy = "TeachersAndAdmins")]
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] QuestionForCreationDto dto)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await this.questionService.CreateAsync(dto)
        });

    [Authorize(Policy = "TeachersAndAdmins")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await this.questionService.RemoveAsync(id)
        });

    [Authorize(Policy = "TeachersAndAdmins")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync([FromRoute(Name = "id")] long id, [FromBody] QuestionForUpdateDto dto)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await this.questionService.ModifyAsync(id, dto)
        });

    [HttpGet("quizid")]
    public async Task<IActionResult> GetByQuizIdAsync(long quizId)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await this.questionService.RetrieveByQuizIdAsync(quizId)
        }
        );
}
