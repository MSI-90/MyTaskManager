using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTaskManager.Models.DTO.TaskDTO;
using MyTaskManager.Repositories.Interfaces;


namespace MyTaskManager.Controllers
{
    //[Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        readonly ITaskRepository _repository;
        public TasksController(ITaskRepository repository) => _repository = repository;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<MyTaskDto>> Get()
        {

            return await _repository.GetAllTasksAsync();
        }

        [HttpGet("{id:min(1)}")]
        public async Task<ActionResult<MyTaskDto>> GetTask(int id)
        {
            var taskById = await _repository.GetTaskAsync(id);
            if (taskById.Id == 0)
                return BadRequest();

            return Ok(taskById);
        }

        [HttpPost]
        public async Task<ActionResult> AddTask([FromBody] CreateTaskRequest newTask)
        {
            await _repository.AddTaskAsync(newTask);

            return CreatedAtAction(nameof(GetTask), new { Id = newTask.Id }, newTask);
        }

        [HttpPut("{id:min(1)}")]
        public async Task<IActionResult> UpdateTask([FromBody] SmallTaskDTO newTask, int id)
        {
            var oldTaskId = await _repository.GetTaskAsync(id);

            if (oldTaskId.Id > 0)
            {
                await _repository.TaskUpdate(oldTaskId.Id, newTask);
                return NoContent();
            }

            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:min(1)}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _repository.GetTaskAsync(id);

            if (task.Id > 0)
            {
                await _repository.Delete(task);
                return Ok();
            }

            return NotFound();
        }
    }
}
