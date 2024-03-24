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
    [Authorize]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IEnumerable<MyTaskDto>> Get()
        {
            return await _repository.GetAllTasksAsync();
        }

        [HttpGet("{id:min(1)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MyTaskDto>> GetTask(int id)
        {
            var taskById = await _repository.GetTaskAsync(id);

            if (taskById.Id <= 0)
            {
                var notFoundMessage = _localization["ErrorOfGetTaskById"].Value;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(notFoundMessage);
                return NotFound(_response);
            }

            return Ok(taskById);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> AddTask([FromBody] CreateTaskRequest newTask)
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
            _response.StatusCode = HttpStatusCode.Created;
            _response.IsSuccess = true;
            _response.Result = responseFromCreateTask;
            return CreatedAtAction(nameof(GetTask), new { Id = responseFromCreateTask.Id }, _response);
        }

        [HttpPut("{id:min(1)}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTask([FromForm] UpdateTaskDTO newTaskForUpdate, int id)
        {
            var oldTaskId = await _repository.GetTaskAsync(id);

            if (oldTaskId.Id <= 0)
            {
                var notFoundMessage = _localization["ErrorOfGetTaskById"].Value;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(notFoundMessage);
                return BadRequest(_response);
            }

            await _repository.TaskUpdate(oldTaskId.Id, newTaskForUpdate);
            return NoContent();
        }

        [HttpDelete("{id:min(1)}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
