using Teydes.Service.DTOs.Submissions;

namespace Teydes.Service.Interfaces.Submissions;

public interface ISubmissionService
{
    public Task<bool> RemoveAsync(long id);
    public Task<SubmissionForResultDto> RetrieveByIdAsync(long id);
    public IEnumerable<SubmissionForResultDto> RetrieveAll();
    public Task<SubmissionForResultDto> CreateAsync(SubmissionForCreationDto dto);
    public Task<SubmissionForResultDto> ModifyAsync(long id, SubmissionForUpdateDto dto);
    public Task<IEnumerable<SubmissionForResultDto>> RetrieveAllAsyncByQuizId(long quizId);
}
