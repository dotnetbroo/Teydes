using Teydes.Domain.Configurations;
using Teydes.Service.DTOs.Questions;

namespace Teydes.Service.Interfaces.Questions;

public interface IQuestionService
{
    public Task<bool> RemoveAsync(long id);
    public Task<QuestionForResultDto> RetrieveAsync(long id);
    public Task<QuestionForResultDto> CreateAsync(QuestionForCreationDto dto);
    public Task<QuestionForResultDto> ModifyAsync(long id, QuestionForUpdateDto dto);
    public Task<IEnumerable<QuestionForResultDto>> RetrieveByQuizIdAsync(long quizId);
    public Task<IEnumerable<QuestionForResultDto>> RetrieveAllAsync(PaginationParams @params);
}
