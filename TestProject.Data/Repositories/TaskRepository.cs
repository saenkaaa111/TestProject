using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TestProject.Data
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskContext _context;
        private readonly ILogger<TaskRepository> _logger;

        public TaskRepository(TaskContext context, ILogger<TaskRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TaskTable?> GetByIdAsync(Guid id) => await _context.TaskTable.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Guid> AddAsync(TaskTable entity)
        {
            try
            {
                await _context.TaskTable.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error creating task. Error: {ex.Message}", ex.Message);
                throw new Exception($"Error creating task. Error: {ex.Message}");
            }
            
        }

        public async Task UpdateTaskStatusAsync(TaskTable entity, Status status)
        {
            entity.Status = status;
            entity.UpdatedAt = DateTime.UtcNow;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating task {id}. Error: {ex.Message}", entity.Id, ex.Message);
                throw new Exception($"Error updating task. Error: {ex.Message}");
            }
        }
    }
}
