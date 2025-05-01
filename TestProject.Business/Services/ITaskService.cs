using TestProject.Data;

namespace TestProject.Business
{
    public interface ITaskService
    {
        Task<Guid> CreateTaskAsync();
        Task<Status?> GetTaskStatusByIdAsync(Guid id);
    }
}