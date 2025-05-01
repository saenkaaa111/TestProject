using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestProject.Data;

namespace TestProject.Business
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TaskService> _logger;

        public TaskService(ITaskRepository taskRepository, IServiceScopeFactory scopeFactory,
            ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task<Guid> CreateTaskAsync()
        {
            var entity = new TaskTable()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = Status.created
            };
            var taskId = await _taskRepository.AddAsync(entity);
            
            _ = ProcessTaskAsync(taskId);
            return taskId;
        }

        public async Task<Status?> GetTaskStatusByIdAsync(Guid id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            return task == null ? null : task.Status;
        }

        private async Task ProcessTaskAsync(Guid id)
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<ITaskRepository>();

            try
            {
                var entity = await repository.GetByIdAsync(id);
                if (entity is null)
                {
                    _logger.LogWarning("Task {id} not found in database", id);
                    return;
                }
                
                await repository.UpdateTaskStatusAsync(entity, Status.running);
                await Task.Delay(TimeSpan.FromMinutes(2));
                await repository.UpdateTaskStatusAsync(entity, Status.finished);
                
            }
            catch (Exception ex)
            {
                _logger.LogError("Error processing task {id}. Error: {ex.Message}", id, ex.Message);
            }
        }
    }
}
