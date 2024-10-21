using AutoMapper;
using Teydes.Data.IRepositories;
using Teydes.Service.DTOs.Courses;
using Teydes.Domain.Configurations;
using Microsoft.EntityFrameworkCore;
using Teydes.Service.Commons.Helpers;
using Teydes.Domain.Entities.Courses;
using Teydes.Service.Commons.Exceptions;
using Teydes.Service.Commons.Extensions;
using Teydes.Service.Interfaces.Courses;

namespace Teydes.Service.Services.Courses;

public class CourseService : ICourseService
{
    private readonly IMapper mapper;
    private readonly IRepository<Course> courseRepository;

    public CourseService(IRepository<Course> courseRepository, IMapper mapper)
    {
        this.mapper = mapper;
        this.courseRepository = courseRepository;
    }

    public async Task<CourseForResultDto> AddAsync(CourseForCreationDto dto)
    {
        var course = await this.courseRepository.SelectAll()
            .Where(u => u.Name == dto.Name)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        if (course is not null)
            throw new CustomException(403, "Course is already exists");

        var forCreation = this.mapper.Map<Course>(dto);
        forCreation.CreatedAt = TimeHelper.GetCurrentServerTime();

        var result = await this.courseRepository.InsertAsync(forCreation);
        await this.courseRepository.SaveAsync();

        return this.mapper.Map<CourseForResultDto>(result);
    }

    public async Task<IEnumerable<CourseForResultDto>> GetAllAsync()
    {
        var courses = await this.courseRepository.SelectAll()
            .AsNoTracking()
            .ToListAsync();

        return this.mapper.Map<IEnumerable<CourseForResultDto>>(courses);
    }

    public async Task<CourseForResultDto> ModifyAsync(long id, CourseForUpdateDto dto)
    {
        var course = await this.courseRepository.SelectAsync(u => u.Id == id);
        if (course is null)
            throw new CustomException(404, "Course not found");

        if(dto is not null)
        {
            course.Name = string.IsNullOrEmpty(dto.Name) ? course.Name : dto.Name;
        
            course.UpdatedAt = TimeHelper.GetCurrentServerTime();

            this.courseRepository.Update(course);
            await this.courseRepository.SaveAsync();
        }

        var person = this.mapper.Map(dto, course);

        return this.mapper.Map<CourseForResultDto>(person);
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var course = await this.courseRepository.SelectAsync(u => u.Id == id);
        if (course is null)
            throw new CustomException(404, "Course not found");

        await this.courseRepository.DeleteAsync(id);
        var result = await this.courseRepository.SaveAsync();

        return result;
    }

    public async Task<IEnumerable<CourseForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var courses = await this.courseRepository.SelectAll()
            .Include(u => u.Groups)
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync();

        return this.mapper.Map<IEnumerable<CourseForResultDto>>(courses);
    }


    public async Task<CourseForResultDto> RetrieveByIdAsync(long id)
    {
        var course = await this.courseRepository.SelectAll()
        .Where(c => c.Id == id)
        .Include(cg => cg.Groups)
        .AsNoTracking()
        .FirstOrDefaultAsync();

        if (course is null)
            throw new CustomException(404, "Course Not Found");

        return this.mapper.Map<CourseForResultDto>(course);
    }

    public async Task<IEnumerable<CourseForResultDto>> SearchAllAsync(string search, PaginationParams @params)
    {
        var courses = await this.courseRepository.SelectAll()
            .Where(x => x.Name.ToLower().Contains(search.ToLower()))
            .Include(u => u.Groups)
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync();

        return this.mapper.Map<IEnumerable<CourseForResultDto>>(courses);
    }
}
