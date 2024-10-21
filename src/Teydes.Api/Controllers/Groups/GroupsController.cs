using Teydes.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Teydes.Service.DTOs.Groups;
using Teydes.Domain.Configurations;
using Teydes.Api.Controllers.Commons;
using Teydes.Service.Interfaces.Groups;
using Microsoft.AspNetCore.Authorization;

namespace Teydes.Api.Controllers.Groups
{
    [Authorize]
    public class GroupsController : BaseController
    {
        private readonly IGroupService groupService;

        public GroupsController(IGroupService groupService)
        {
            this.groupService = groupService;
        }

        ///<summary>
        ///Create group 
        ///</summary>
        ///<param name="groupForCreationDto"></param>
        ///<returns></returns>
        [Authorize(Policy = "Admins")]
        [HttpPost]
        public async Task<ActionResult<GroupForResultDto>> postAsync(GroupForCreationDto groupForCreationDto)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await groupService.AddAsync(groupForCreationDto)
            });

        /// <summary>
        /// Get all groups
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await groupService.RetrieveAllAsync(@params)
        });

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute(Name = "id")] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await groupService.RetrieveByIdAsync(id)
            });

        /// <summary>
        /// Update group info
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize(Policy = "Admins")]
        [HttpPut("{id}")]
        public async Task<ActionResult<GroupForResultDto>> PutAsync([FromRoute(Name = "id")] long id, [FromBody] GroupForUpdateDto dto)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await groupService.ModifyAsync(id, dto)
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
                Data = await groupService.RemoveAsync(id)
            });

        [Authorize(Policy = "TeachersAndAdmins")]
        [HttpGet("id")]
        public async Task<IActionResult> GetGroupStudentwithOrderAsync(long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await groupService.RetrieveByIdWithStudentRankingAsync(id)
            });
    }
}
