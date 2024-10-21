using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Teydes.Service.DTOs.Courses;
using Teydes.Domain.Configurations;
using Teydes.Service.Commons.Exceptions;
using Teydes.Service.Interfaces.Courses;
using Teydes.Service.DTOs.Users;
using Teydes.Service.Services.Users;

namespace Teydes.Web.Controllers;

public class CourseController : BaseController
{
    private readonly IMapper mapper;
    private readonly ICourseService courseService;

    public CourseController
        (
        IMapper mapper,
        ICourseService courseService
        )
    {
        this.mapper = mapper;
        this.courseService = courseService;
    }

    #region GetAll
    [HttpGet("Courses")]
    public async Task<ViewResult> Index(string search, int page = 1)
    {
        var paginationParams = new PaginationParams
        {
            PageSize = 30,
            PageIndex = page
        };

        List<CourseForResultDto> courses;

        if (!string.IsNullOrEmpty(search))
        {
            ViewBag.search = search;

            courses = (await courseService.SearchAllAsync(search, paginationParams)).ToList();
            ViewBag.courses = courses;

            return View("Index");
        }

        courses = (await courseService.RetrieveAllAsync(paginationParams)).ToList();
        ViewBag.courses = courses;

        return View("Index");
    }
    #endregion

    #region Create
    [HttpGet("CreateCourse")]
    public async Task<ViewResult> CreateCourseRedirect()
    {
        return View("Create");
    }

    [HttpPost("CreateCourse")]
    public async Task<IActionResult> CreateCourseAsync(CourseForCreationDto dto)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var course = await courseService.AddAsync(dto);
                if (course is null)
                {
                    return await CreateCourseRedirect();
                }
                return RedirectToAction("Index");
            }
            return await CreateCourseRedirect();
        }
        catch (CustomException ex)
        {
            ModelState.AddModelError(dto.Name, ex.Message);
            return await CreateCourseRedirect();
        }
    }
    #endregion

    #region Update
    [HttpGet("UpdateCourse")]
    public async Task<ViewResult> UpdateRedirectAsync(long courseId)
    {
        var course = await this.courseService.RetrieveByIdAsync(courseId);

        var dto = new CourseForUpdateDto()
        {
            Id = courseId,
            Name = course.Name
        };

        ViewBag.Id = course.Id;

        return View("Update", dto);
    }

    [HttpPost("UpdateCourse")]
    public async Task<IActionResult> UpdateCourseAsync(CourseForUpdateDto dto, long courseId)
    {
        if (ModelState.IsValid)
        {
            var model = await this.courseService.ModifyAsync(courseId, dto);
            if (model is null)
            {
                return await this.UpdateRedirectAsync(courseId);
            }
            return RedirectToAction("Index");
        }
        return await this.UpdateRedirectAsync(courseId);
    }
    #endregion

    #region Remove 
    [HttpGet("DeleteCourse")]
    public async Task<ViewResult> DeleteRedirectAsync(long id)
    {
        var course = await this.courseService.RetrieveByIdAsync(id);
        return View("Delete", course);
    }

    [HttpPost("DeleteCourse")]
    public async Task<IActionResult> DeleteCourseAsync(long id)
    {
        var course = await this.courseService.RetrieveByIdAsync(id);
        if (course is null)
            throw new CustomException(404, "Course not found");

        var result = await this.courseService.RemoveAsync(id);
        if (result) return RedirectToAction("Index");

        return await DeleteRedirectAsync(id);
    }
    #endregion

    #region Get
    [HttpGet("GetCourse")]
    public async Task<IActionResult> GetByIdAsync(long courseId)
    {
        try
        {
            var course = await courseService.RetrieveByIdAsync(courseId);
            var courseViewModel = new CourseForResultDto()
            {
                Id = course.Id,
                Name = course.Name,
                Groups = course.Groups
            };

            return View("Profile", courseViewModel);
        }
        catch (CustomException exception)
        {
            return View("ErrorPages/NotFound", exception);
        }
    }
    #endregion
}
