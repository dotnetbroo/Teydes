using Teydes.Service.DTOs.Quizzes;
using Teydes.Domain.Configurations;
using Teydes.Service.DTOs.QuizResults;

namespace Teydes.Service.Interfaces.QuizResults;

public interface IQuizResultService
{
  public Task<QuizResultForResultDto> RetrieveStudentQuizResultAsync(long quizId, long studentId);
}
