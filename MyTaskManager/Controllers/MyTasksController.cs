using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTaskManager.Data;
using MyTaskManager.Repositories.Interfaces;


namespace MyTaskManager.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MyTasksController : ControllerBase
    {
        readonly ITaskRepository _repository;
        public MyTasksController(ITaskRepository repository) => _repository = repository;

        [HttpGet("{id}")]
        public async Task<ActionResult<MyTask>> GetTask(int id)
        {
            return await _repository.GetTaskAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<MyTask>> AddTask(MyTask newTask)
        {
            await _repository.AddTaskAsync(newTask);

            return CreatedAtAction("GetTask", new { Id =  newTask.Id }, newTask);
        }
    }
}
