using Teydes.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Teydes.Service.Interfaces.Assets;
using System.ComponentModel.DataAnnotations;
using Teydes.Api.Controllers.Commons;
using Microsoft.AspNetCore.Authorization;

namespace Teydes.Api.Controllers.Assets
{
    [Authorize]
    public class AssetsController : BaseController
    {
        private readonly IAssetService assetService;

        public AssetsController(IAssetService assetService)
        {
            this.assetService = assetService;
        }

        [Authorize(Policy = "TeachersAndAdmins")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await this.assetService.RemoveAsync(id)
            });

        [Authorize(Policy = "TeachersAndAdmins")]
        [HttpPost("upload")]
        public async Task<IActionResult> PostAsync(
            [Required(ErrorMessage = "Please, select file ...")]
            [DataType(DataType.Upload)] IFormFile file)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await this.assetService.UploadAsync(file)
            });

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await this.assetService.RetriveByIdAsync(id)
            });
    }
}
