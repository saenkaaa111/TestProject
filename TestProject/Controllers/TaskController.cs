using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TestProject.Business;

namespace TestProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get task status by id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(string))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<string>> GetById(string id)
        {
            if (!Guid.TryParse(id, out var taskId))
                return BadRequest("Invalid GUID format");

            var status = await _taskService.GetTaskStatusByIdAsync(taskId);

            return status == null ? NotFound() : Ok(status.ToString());            
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create new task")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult<Guid>> CreateTask()
        {
            var taskId = await _taskService.CreateTaskAsync();
            return Accepted(taskId);
        }
    }
}

