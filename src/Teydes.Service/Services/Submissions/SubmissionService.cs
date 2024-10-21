using AutoMapper;
using Teydes.Data.IRepositories;
using Teydes.Domain.Entities.Users;
using Teydes.Domain.Entities.Quizes;
using Microsoft.EntityFrameworkCore;
using Teydes.Service.DTOs.Submissions;
using Teydes.Service.Commons.Exceptions;
using Teydes.Service.Interfaces.Submissions;

namespace Teydes.Service.Services.Submissions;

public class SubmissionService : ISubmissionService
{
    private readonly IMapper mapper;
    private readonly IRepository<User> userRepository;
    private readonly IRepository<Quiz> quizRepository;
    private readonly IRepository<Submission> sumbissionRepository;
    private readonly IRepository<QuestionAnswer> questionAnswerRepository;

    public SubmissionService(
        IMapper mapper, 
        IRepository<User> userRepository,
        IRepository<Quiz> quizRepository,
        IRepository<QuestionAnswer> answerRepository,
        IRepository<Submission> sumbissionRepository)
    {
        this.mapper = mapper;
        this.userRepository = userRepository;
        this.quizRepository = quizRepository;
        this.questionAnswerRepository = answerRepository;
        this.sumbissionRepository = sumbissionRepository;
    }
    public async Task<SubmissionForResultDto> CreateAsync(SubmissionForCreationDto dto)
    {
        var user = await this.userRepository.SelectAsync(u => u.Id == dto.UserId);
        if (user is null)
            throw new CustomException(404, "User is not found");

        var quiz = await this.quizRepository
          .SelectAll(q => q.Id == dto.QuizId)
          .Include(q => q.Questions.Where(question => question.Id == dto.QuestionId).Take(1))
          .ThenInclude(question => question.QuestionAnswers)
          .AsNoTracking() 
          .FirstOrDefaultAsync();
        if (quiz is null)
            throw new CustomException(404, "Quiz is not found");

        if (quiz?.Questions is null)
            throw new CustomException(404, "Question is not found");

        var questionAnswer = await this.questionAnswerRepository.SelectAll()
             .Where(qa => qa.QuestionId == dto.QuestionId && qa.Id == dto.QuestionOptionId)
             .AsNoTracking()
             .FirstOrDefaultAsync();
        if (questionAnswer is null)
            throw new CustomException(404, "QuestionAnswer is not found");

        var mappedSumbission = this.mapper.Map<Submission>(dto);
        if (questionAnswer.IsCorrect)
            mappedSumbission.IsCorrect = true;

        var result = await this.sumbissionRepository.InsertAsync(mappedSumbission);
        await this.sumbissionRepository.SaveAsync();

        return this.mapper.Map<SubmissionForResultDto>(result);
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var submission = await this.sumbissionRepository.SelectAsync(s => s.Id == id);
        if (submission is null)
            throw new CustomException(404, "Submission is not found");

        await this.sumbissionRepository.DeleteAsync(id);
        await this.sumbissionRepository.SaveAsync();

        return true;
    }

    public async Task<SubmissionForResultDto> ModifyAsync(long id, SubmissionForUpdateDto dto)
    {
        var submission = await this.sumbissionRepository.SelectAsync(s => s.Id == id);
        if (submission is null)
            throw new CustomException(404, "Submission is not found");

        var mappedSubmission = this.mapper.Map(dto, submission);
        await this.sumbissionRepository.SaveAsync();

        return this.mapper.Map<SubmissionForResultDto>(submission);
    }

    public IEnumerable<SubmissionForResultDto> RetrieveAll()
    {
        var submissions = this.sumbissionRepository.SelectAll()
            .Include(u => u.User)
            .Include(q => q.Question)
            .ThenInclude(qz => qz.Quiz)
            .Include(qo => qo.QuestionOption)
            .OrderByDescending(submission => submission.Id)
            .AsNoTracking();

        return this.mapper.Map<IEnumerable<SubmissionForResultDto>>(submissions);
    }

    public async Task<SubmissionForResultDto> RetrieveByIdAsync(long id)
    {
        var submission = await this.sumbissionRepository.SelectAll()
            .Where(s => s.Id == id)
            .Include(u => u.User)
            .Include(q => q.Question)
            .ThenInclude(qz => qz.Quiz)
            .Include(qo => qo.QuestionOption)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        if (submission is null)
            throw new CustomException(404, "Sumbission is not found");

        return this.mapper.Map<SubmissionForResultDto>(submission);
    }

    public Task<IEnumerable<SubmissionForResultDto>> RetrieveAllAsyncByQuizId(long quizId)
    {
        throw new NotImplementedException();
    }
}
