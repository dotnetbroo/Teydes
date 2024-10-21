using Teydes.Domain.Configurations;
using Teydes.Service.DTOs.Quizzes;

namespace Teydes.Service.Interfaces.Quizzes;

public interface IQuizService
{
    public Task<bool> RemoveAsync(long id);
    public Task<QuizForResultDto> RetrieveByIdAsync(long id);
    public Task<QuizForResultDto> CreateAsync(QuizForCreationDto dto);
    public Task<QuizForResultDto> ModifyAsync(long id, QuizForUpdateDto dto);
    public Task<IEnumerable<QuizForResultDto>> RetrieveAllAsync(PaginationParams @params);
}
