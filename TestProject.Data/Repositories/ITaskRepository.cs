namespace TestProject.Data
{
    public interface ITaskRepository
    {
        Task<Guid> AddAsync(TaskTable entity);
        Task<TaskTable?> GetByIdAsync(Guid id);
        Task UpdateTaskStatusAsync(TaskTable entity, Status status);
    }
}