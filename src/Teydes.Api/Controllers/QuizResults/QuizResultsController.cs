using Microsoft.AspNetCore.Mvc;
using Teydes.Api.Controllers.Commons;
using Teydes.Shared.Models;
using Teydes.Service.Interfaces.QuizResults;
using Microsoft.AspNetCore.Authorization;

namespace Teydes.Api.Controllers.QuizResults;

[Authorize]
public class QuizResultsController : BaseController
{
    private readonly IQuizResultService quizResultService;

    public QuizResultsController(IQuizResultService quizResultService)
    {
        this.quizResultService = quizResultService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(long quizId,long studentId)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await this.quizResultService.RetrieveStudentQuizResultAsync(quizId, studentId)
        });
}
