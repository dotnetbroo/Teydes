using Teydes.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Teydes.Service.DTOs.Users;
using Teydes.Service.DTOs.Courses;
using Teydes.Domain.Configurations;
using Teydes.Api.Controllers.Commons;
using Teydes.Service.Interfaces.Courses;
using Microsoft.AspNetCore.Authorization;

namespace Teydes.Api.Controllers.Courses
{
    [Authorize]
    public class CoursesController : BaseController
    {
        private readonly ICourseService courseService;

        public CoursesController(ICourseService courseService)
        {
            this.courseService = courseService;
        }

        ///<summary>
        ///Create course 
        ///</summary>
        ///<param name="courseForCreationDto"></param>
        ///<returns></returns>
        [Authorize(Policy = "Admins")]
        [HttpPost]
        public async Task<ActionResult<CourseForResultDto>> PostAsync(CourseForCreationDto courseForCreationDto)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await courseService.AddAsync(courseForCreationDto)
            });

        /// <summary>
        /// Get all courses
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await courseService.RetrieveAllAsync(@params)
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
                Data = await courseService.RetrieveByIdAsync(id)
            });

        /// <summary>
        /// Update course info
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize(Policy = "Admins")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserForResultDto>> PutAsync([FromRoute(Name = "id")] long id, [FromBody] CourseForUpdateDto dto)
            => Ok(new Response
            {
                Code = 200,
                Message = "Success",
                Data = await courseService.ModifyAsync(id, dto)
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
                Data = await courseService.RemoveAsync(id)
            });
    }
}
