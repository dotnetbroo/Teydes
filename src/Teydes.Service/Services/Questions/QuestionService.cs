using AutoMapper;
using Teydes.Data.IRepositories;
using Teydes.Domain.Configurations;
using Teydes.Domain.Entities.Quizes;
using Teydes.Service.DTOs.Questions;
using Microsoft.EntityFrameworkCore;
using Teydes.Domain.Entities.Assets;
using Teydes.Service.Commons.Exceptions;
using Teydes.Service.Commons.Extensions;
using Teydes.Service.Interfaces.Questions;

namespace Teydes.Service.Services.Questions;

public class QuestionService : IQuestionService
{
    private readonly IMapper mapper;
    private readonly IRepository<Quiz> quizRepository;
    private readonly IRepository<Asset> assetRepository;
    private readonly IRepository<Question> questionRepository;

    public QuestionService(
        IMapper mapper, 
        IRepository<Quiz> quizRepository,
        IRepository<Asset> assetRepository,
        IRepository<Question> questionRepository)
    {
        this.mapper = mapper;
        this.quizRepository = quizRepository;
        this.assetRepository = assetRepository;
        this.questionRepository = questionRepository;
    }
    public async Task<QuestionForResultDto> CreateAsync(QuestionForCreationDto dto)
    {
        var quiz = await this.quizRepository.SelectAsync(q => q.Id == dto.QuizId);
        var asset = await this.assetRepository.SelectAsync(a => a.Id == dto.AssetId);
        if (quiz is null)
            throw new CustomException(404, "Quiz is not found");
        if (dto.AssetId != null && asset is null)
            throw new CustomException(404, "Asset is not found");

        var question = this.mapper.Map<Question>(dto);
        question.CreatedAt = DateTime.UtcNow;
        var result = await this.questionRepository.InsertAsync(question);
        await this.questionRepository.SaveAsync();

        return this.mapper.Map<QuestionForResultDto>(result);   
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var question = await this.questionRepository.SelectAsync(q => q.Id ==  id);
        if (question is null)
            throw new CustomException(404, "Question is not found");

        await this.questionRepository.DeleteAsync(id);
        await this.questionRepository.SaveAsync();

        return true;
    }

    public async Task<IEnumerable<QuestionForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var questions = await this.questionRepository.SelectAll()
            .Include(q => q.Quiz)
            .ThenInclude(qz => qz.Group)
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync();

        var shuffleQuestions = questions.Shuffle();

        return this.mapper.Map<IEnumerable<QuestionForResultDto>>(shuffleQuestions);
    }


    public async Task<QuestionForResultDto> RetrieveAsync(long id)
    {
        var question = await this.questionRepository.SelectAll()
            .Where(q => q.Id == id)
            .Include(qz => qz.Quiz)
            .ThenInclude(qg => qg.Group)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        if (question is null)
            throw new CustomException(404, "Question is not found");

        return this.mapper.Map<QuestionForResultDto>(question);
    }

    public async Task<QuestionForResultDto> ModifyAsync(long id, QuestionForUpdateDto dto)
    {
        var question = await this.questionRepository.SelectAsync(q => q.Id == id);
        if (question is null)
            throw new CustomException(404, "Question is not found");

        var quiz = await this.quizRepository.SelectAsync(q => q.Id == dto.QuizId);
        if (quiz is null)
            throw new CustomException(404, "Quiz is not found");

        //var asset = await this.assetRepository.SelectAsync(a => a.Id == dto.AssetId);
        //if (dto.AssetId != 0 &&  asset is null)
        //    throw new CustomException(404, "Asset is not found");

        var result = this.mapper.Map(dto, question);
        result.UpdatedAt = DateTime.UtcNow;
        await this.questionRepository.SaveAsync();

        return this.mapper.Map<QuestionForResultDto>(result);
    }

    public async Task<IEnumerable<QuestionForResultDto>> RetrieveByQuizIdAsync(long quizId)
    {
        var questions = await this.questionRepository.SelectAll()
            .Where(q => q.QuizId == quizId)
            .Include(q => q.Quiz)
            .AsNoTracking()
            .ToListAsync();

        var randomQuestions = questions.Shuffle();

        return this.mapper.Map<IEnumerable<QuestionForResultDto>>(randomQuestions);
    }
}
