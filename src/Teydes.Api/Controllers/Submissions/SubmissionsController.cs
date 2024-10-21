using Teydes.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Teydes.Api.Controllers.Commons;
using Teydes.Service.DTOs.Submissions;
using Teydes.Service.Interfaces.Submissions;
using Microsoft.AspNetCore.Authorization;

namespace Teydes.Api.Controllers.Submissions;

[Authorize]
public class SubmissionsController : BaseController
{
    private readonly ISubmissionService submissionService;

    public SubmissionsController(ISubmissionService submissionService)
    {
        this.submissionService = submissionService;
    }

    [HttpGet]
    public IActionResult GetAll()
          => Ok(new Response
          {
              Code = 200,
              Message = "Success",
              Data =  this.submissionService.RetrieveAll()
          });

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute(Name = "id")] long id)
           => Ok(new Response
           {
               Code = 200,
               Message = "OK",
               Data = await submissionService.RetrieveByIdAsync(id)
           });

    [HttpPost]
    public async Task<IActionResult> PostAsync(SubmissionForCreationDto dto)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await this.submissionService.CreateAsync(dto)
        });

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync([FromRoute(Name = "id")] long id, [FromBody] SubmissionForUpdateDto dto)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await this.submissionService.ModifyAsync(id, dto)
        });

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await this.submissionService.RemoveAsync(id)
        });

}
