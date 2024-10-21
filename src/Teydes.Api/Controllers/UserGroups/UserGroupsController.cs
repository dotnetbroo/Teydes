using Teydes.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Teydes.Domain.Configurations;
using Teydes.Service.DTOs.UserGroups;
using Teydes.Api.Controllers.Commons;
using Teydes.Service.Interfaces.UserGroups;
using Microsoft.AspNetCore.Authorization;

namespace Teydes.Api.Controllers.UserGroups
{
    [Authorize]
    public class UserGroupsController : BaseController
    {
        private readonly IUserGroupService userGroupService;

        public UserGroupsController(IUserGroupService userGroupService)
        {
            this.userGroupService = userGroupService;
        }

        ///<summary>
        ///Add users at group 
        ///</summary>
        ///<param name="userGroupForCreationDto"></param>
        ///<returns></returns>
        [Authorize(Policy = "TeachersAndAdmins")]
        [HttpPost]
        public async Task<ActionResult<UserGroupForResultDto>> PostAsync(UserGroupForCreationDto userGroupForCreationDto)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await userGroupService.AddAsync(userGroupForCreationDto)
            });

        /// <summary>
        /// Get all groups
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
            Data = await userGroupService.RetrieveAllAsync(@params)
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
                Data = await userGroupService.RetrieveByIdAsync(id)
            });

        /// <summary>
        /// Update group info
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize(Policy = "TeachersAndAdmins")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserGroupForResultDto>> PutAsync([FromRoute(Name = "id")] long id, [FromBody] UserGroupForUpdateDto dto)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await userGroupService.ModifyAsync(id, dto)
            });

        /// <summary>
        /// Delete by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Policy = "TeachersAndAdmins")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteAsync([FromRoute(Name = "id")] long id)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await userGroupService.RemoveAsync(id)
            });
    }
}
