using Teydes.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Teydes.Service.DTOs.Quizzes;
using Teydes.Domain.Configurations;
using Teydes.Api.Controllers.Commons;
using Teydes.Service.Interfaces.Quizzes;
using Microsoft.AspNetCore.Authorization;

namespace Teydes.Api.Controllers.Quizzes
{
    [Authorize]
    public class QuizzesController : BaseController
    {
        private readonly IQuizService quizService;
        public QuizzesController(IQuizService quizService)
        {
            this.quizService = quizService;
        }

        [Authorize(Policy = "TeachersAndAdmins")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await this.quizService.RetrieveAllAsync(@params)
            });

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute(Name = "id")] long id)
               => Ok(new Response
               {
                   Code = 200,
                   Message = "OK",
                   Data = await quizService.RetrieveByIdAsync(id)
               });

        [Authorize(Policy = "TeachersAndAdmins")]
        [HttpPost]
        public async Task<IActionResult> PostAsync(QuizForCreationDto dto)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await this.quizService.CreateAsync(dto)
            });

        [Authorize(Policy = "TeachersAndAdmins")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute(Name = "id")] long id,[FromBody] QuizForUpdateDto dto)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await this.quizService.ModifyAsync(id, dto)
            });

        [Authorize(Policy = "TeachersAndAdmins")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await this.quizService.RemoveAsync(id)
            });
    }
}
