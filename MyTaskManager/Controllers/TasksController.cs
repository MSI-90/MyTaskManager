using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MyTaskManager.Models;
using MyTaskManager.Models.DTO.TaskDTO;
using MyTaskManager.Repositories.Interfaces;
using System.Net;

namespace MyTaskManager.Controllers
{
    //[Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _repository;
        private readonly IStringLocalizer<TasksController> _localization;
        protected APIResponse _response;
        public TasksController(ITaskRepository repository, IStringLocalizer<TasksController> localization)
        {
            _repository = repository;
            _localization = localization;
            this._response = new();
        }

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddTask([FromForm] CreateTaskRequest newTask)
        {
            var responseFromCreateTask = await _repository.AddTaskAsync(newTask);
            if (responseFromCreateTask.Id == 0 || string.IsNullOrEmpty(responseFromCreateTask.TitleTask))
            {
                var errorMEssage = _localization["ErrorOfAddingTask"].Value;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(errorMEssage);
                _response.Result = responseFromCreateTask;
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = responseFromCreateTask;
            return Ok(_response);
        }
        //Old realisation
        //return CreatedAtAction(nameof(GetTask), new { Id = newTask.Id }, newTask);

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
