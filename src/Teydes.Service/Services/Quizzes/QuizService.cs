using AutoMapper;
using Teydes.Data.IRepositories;
using Teydes.Service.DTOs.Quizzes;
using Teydes.Domain.Configurations;
using Teydes.Domain.Entities.Quizes;
using Microsoft.EntityFrameworkCore;
using Teydes.Domain.Entities.Courses;
using Teydes.Service.Interfaces.Quizzes;
using Teydes.Service.Commons.Exceptions;
using Teydes.Service.Commons.Extensions;

namespace Teydes.Service.Services.Quizzes;

public class QuizService : IQuizService
{
    private readonly IMapper mapper;
    private readonly IRepository<Quiz> quizRepository;
    private readonly IRepository<Group> groupRepository;
    public QuizService(
        IMapper mapper,
        IRepository<Quiz> quizRepository,
        IRepository<Group> groupRepository)
    {
        this.mapper = mapper;
        this.quizRepository = quizRepository;
        this.groupRepository = groupRepository;
    }
    public async Task<QuizForResultDto> CreateAsync(QuizForCreationDto dto)
    {
        var group = await this.groupRepository.SelectAsync(g => g.Id == dto.GroupId);
        if (group is null)
            throw new CustomException(404, "Group is not available");

        var quiz = this.mapper.Map<Quiz>(dto);
        quiz.CreatedAt = DateTime.UtcNow;
        var result = await this.quizRepository.InsertAsync(quiz);
        await this.quizRepository.SaveAsync();

        return this.mapper.Map<QuizForResultDto>(result);
    }

    public async Task<QuizForResultDto> ModifyAsync(long id, QuizForUpdateDto dto)
    {
        var quiz = await this.quizRepository.SelectAsync(q => q.Id == id);
        if (quiz is null)
            throw new CustomException(404, "Quiz is not found");

        var group = await this.groupRepository.SelectAsync(g => g.Id == dto.GroupId);
        if (group is null)
            throw new CustomException(404, "Group is not available");

        this.mapper.Map(dto, quiz);
        quiz.UpdatedAt = DateTime.UtcNow;
        await this.quizRepository.SaveAsync();

        return this.mapper.Map<QuizForResultDto>(quiz);
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var quiz = await this.quizRepository.SelectAsync(q => q.Id == id);
        if (quiz is null)
            throw new CustomException(404, "Quiz is not available");

        await this.quizRepository.DeleteAsync(id);
        await this.quizRepository.SaveAsync();
        return true;
    }

    public async Task<IEnumerable<QuizForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var quizzes = await this.quizRepository.SelectAll()
            .Include(g => g.Group)
            .AsNoTracking()   
            .ToPagedList(@params)
            .ToListAsync();

        return this.mapper.Map<IEnumerable<QuizForResultDto>>(quizzes);
    }

    public async Task<QuizForResultDto> RetrieveByIdAsync(long id)
    {
        var quiz = await this.quizRepository.SelectAll()
            .Where(q => q.Id == id)
            .Include(g => g.Group)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        if (quiz is null)
            throw new CustomException(404, "Quiz is not available");

        return this.mapper.Map<QuizForResultDto>(quiz);
    }
}
