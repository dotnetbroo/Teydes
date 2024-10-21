using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Teydes.Data.IRepositories;
using Teydes.Domain.Entities.Quizes;
using Teydes.Domain.Entities.Users;
using Teydes.Service.Commons.Exceptions;
using Teydes.Service.DTOs.Answers;
using Teydes.Service.DTOs.Questions;
using Teydes.Service.DTOs.QuizResults;
using Teydes.Service.DTOs.Submissions;
using Teydes.Service.Interfaces.QuizResults;

namespace Teydes.Service.Services.QuizResults;

public class QuizResultService : IQuizResultService
{
    private readonly IMapper mapper;
    private readonly IRepository<Quiz> quizRepository;
    private readonly IRepository<User> userRepository;
    private readonly IRepository<Submission> submissionRepository;
    private readonly IRepository<QuizResult> quizResultRepository;

    public QuizResultService(
        IMapper mapper,
        IRepository<Quiz> quizRepository,
        IRepository<User> userRepository,
        IRepository<QuizResult> quizResultRepository,
        IRepository<Submission> submissionRepository)
    {
        this.mapper = mapper;
        this.quizRepository = quizRepository;
        this.userRepository = userRepository;
        this.quizResultRepository = quizResultRepository;
        this.submissionRepository = submissionRepository;
    }

    public async Task<QuizResultForResultDto> RetrieveStudentQuizResultAsync(long quizId, long studentId)
    {
        var student = await this.userRepository.SelectAsync(s => s.Id == studentId);
        if (student is null)
            throw new CustomException(404, "Student is not found");

        var quizResult = new QuizResult();

        var quiz = await this.quizRepository.SelectAll(q => q.Id == quizId )
            .Include(q => q.Questions)
            .ThenInclude(q => q.QuestionAnswers.Where(qa => qa.IsCorrect))
            .Include(q => q.Submissions.Where(s => s.UserId == studentId))
            .ThenInclude(s => s.Question)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        if (quiz is null)
            throw new CustomException(404, "Quiz is not found");

        var correctAnswers = quiz?.Submissions.Where(s => s.IsCorrect) .DistinctBy(s => s.QuestionId).Count();
       
        quizResult.CorrectAnswers = (int)correctAnswers;
        quizResult.Score = (double)(quiz.Questions.Count != 0 ? correctAnswers * 100 / quiz.Questions.Count : 0);
        quizResult.UserId = student.Id;
        quizResult.QuizId = quizId;
        quizResult.CreatedAt = DateTime.UtcNow;
        var result = await this.quizResultRepository.InsertAsync(quizResult);
        await this.quizResultRepository.SaveAsync();

        var mappedSubmissions = this.mapper.Map<ICollection<SubmissionForQuizResultDto>>(quiz?.Submissions);
        var mappedQuestion = this.mapper.Map<ICollection<QuestionForQuizResultDto>>(quiz?.Questions);

        var mappedResult =  this.mapper.Map<QuizResultForResultDto>(result);

        var submissions = await this.submissionRepository.SelectAll()
            .Where(s => s.UserId == studentId && s.QuizId == quizId)
            .Include(s => s.QuestionOption)
            .AsNoTracking()
            .ToListAsync();
        foreach(var sub in mappedSubmissions)
        {
            var res = submissions.Where(s => s.Id == sub.Id).FirstOrDefault();
            sub.QuestionOption = this.mapper.Map<QuestionAnswerForResultDto>(res?.QuestionOption);
        }
        mappedResult.Submissions = mappedSubmissions;
        mappedResult.Questions = mappedQuestion;

        return mappedResult;
    }
}
