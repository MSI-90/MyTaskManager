using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTaskManager.Models.DTO.TaskDTO;
using MyTaskManager.Repositories.Interfaces;


namespace MyTaskManager.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        readonly ITaskRepository _repository;
        public TasksController(ITaskRepository repository) => _repository = repository;

        [HttpGet]
        public async Task<IEnumerable<MyTaskDto>> Get()
        {
            return await _repository.GetAllTasksAsync();
        }

        [HttpGet("{id:min(1)}")]
        public async Task<ActionResult<MyTaskDto>> GetTask(int id)
        {
            return await _repository.GetTaskAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult> AddTask([FromForm] MyTaskDto newTask)
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
