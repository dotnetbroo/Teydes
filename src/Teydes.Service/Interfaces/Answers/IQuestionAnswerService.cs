using Teydes.Service.DTOs.Answers;
using Teydes.Domain.Configurations;

namespace Teydes.Service.Interfaces.Answers;

public interface IQuestionAnswerService
{
    public Task<bool> RemoveAsync(long id);
    public Task<QuestionAnswerForResultDto> RetrieveByIdAsync(long id);
    public Task<QuestionAnswerForResultDto> CreateAnswer(QuestionAnswerForCreationDto dto);
    public Task<QuestionAnswerForResultDto> ModifyAnswer(long id, QuestionAnswerForUpdateDto dto);
    public Task<IEnumerable<QuestionAnswerForResultDto>> RetrieveByQuestionIdAsync(long questionId);
    public Task<IEnumerable<QuestionAnswerForResultDto>> RetrieveAllAsync(PaginationParams @params);
}
