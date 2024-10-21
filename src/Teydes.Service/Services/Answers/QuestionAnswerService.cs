using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Teydes.Data.IRepositories;
using Teydes.Domain.Configurations;
using Teydes.Domain.Entities.Quizes;
using Teydes.Service.Commons.Exceptions;
using Teydes.Service.Commons.Extensions;
using Teydes.Service.DTOs.Answers;
using Teydes.Service.DTOs.Questions;
using Teydes.Service.Interfaces.Answers;

namespace Teydes.Service.Services.Answers;

public class QuestionAnswerService : IQuestionAnswerService
{
    private readonly IMapper mapper;
    private readonly IRepository<QuestionAnswer> answerRepository;
    private readonly IRepository<Question> questionRepository;
    public QuestionAnswerService(
        IMapper mapper, 
        IRepository<QuestionAnswer> answerRepository,
        IRepository<Question> questionRepository)
    {
        this.mapper = mapper;
        this.answerRepository = answerRepository;
        this.questionRepository = questionRepository;

    }
    public async Task<QuestionAnswerForResultDto> CreateAnswer(QuestionAnswerForCreationDto dto)
    {
        var question = await this.questionRepository.SelectAsync(q => q.Id == dto.QuestionId);
        if (question is null)
            throw new CustomException(404, "Question is not found");

        var answer = this.mapper.Map<Domain.Entities.Quizes.QuestionAnswer>(dto);
        answer.CreatedAt = DateTime.UtcNow;
        var result = await this.answerRepository.InsertAsync(answer);
        await this.questionRepository.SaveAsync();

        return this.mapper.Map<QuestionAnswerForResultDto>(result);
    }

    public async Task<QuestionAnswerForResultDto> ModifyAnswer(long id, QuestionAnswerForUpdateDto dto)
    {
        var answer = await this.answerRepository.SelectAsync(a => a.Id ==  id);
        if (answer is null)
            throw new CustomException(404, "Answer is not found");

        var question = await this.questionRepository.SelectAsync(q => q.Id == dto.QuestionId);
        if (question is null)
            throw new CustomException(404, "Question is not found");

        var mappedAnswer = this.mapper.Map(dto, answer);
        mappedAnswer.UpdatedAt = DateTime.UtcNow;
        await this.answerRepository.SaveAsync();

        return this.mapper.Map<QuestionAnswerForResultDto>(mappedAnswer);
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var answer = await this.answerRepository.SelectAsync(a => a.Id == id);
        if (answer is null)
            throw new CustomException(404, "Answer is not found");

        await this.answerRepository.DeleteAsync(id);
        await this.answerRepository.SaveAsync();

        return true;
    }

    public async Task<IEnumerable<QuestionAnswerForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var answers = await this.answerRepository.SelectAll()
            .Include(q => q.Question)
            .ThenInclude(qz => qz.Quiz)
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync();

        return this.mapper.Map<IEnumerable<QuestionAnswerForResultDto>>(answers);
    }

    public async Task<QuestionAnswerForResultDto> RetrieveByIdAsync(long id)
    {
        var answer = await this.answerRepository.SelectAll()
            .Where(a => a.Id == id)
            .Include(q => q.Question)
            .ThenInclude(qz => qz.Quiz)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        if (answer is null)
            throw new CustomException(404, "Answer is not found");

        return this.mapper.Map<QuestionAnswerForResultDto>(answer);
    }

    public async Task<IEnumerable<QuestionAnswerForResultDto>> RetrieveByQuestionIdAsync(long questionId)
    {
        var answers = await this.answerRepository.SelectAll()
            .Where(q => q.QuestionId == questionId)
            .Include(q => q.Question)
            .AsNoTracking()
            .ToListAsync();

        return this.mapper.Map<IEnumerable<QuestionAnswerForResultDto>>(answers);
    }
}
