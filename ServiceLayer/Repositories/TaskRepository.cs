using DataLayer.DTO;
using DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using MyTaskManager.Data;
using MyTaskManager.DTO;
using MyTaskManager.Repositories.Interfaces;
using ServiceLayer.Services;

namespace MyTaskManager.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskContext _context;
        public TaskRepository(TaskContext context) => _context = context;

        public async Task<IEnumerable<MyTaskDto>> GetAllTasksAsync()
        {
            var modelFromEntity =  await _context.Tasks
                .Include(c => c.Category)
                .Include(p => p.Priory)
                .ToListAsync(); 

            var taskDto = modelFromEntity.Select(task => new MyTaskDto
            {
                Id = task.Id,
                TitleTask = task.TitleTask,
                Category = task.Category.Name,
                CategoryDescription = task.Category.Description,
                Prior = (PriorityFrom)Enum.Parse(typeof(PriorityFrom), task.Priory.Name),
                Expiration = task.Expiration
            });

            return taskDto.OrderBy(task => task.Id);
        }

        public async Task<MyTaskDto> GetTaskAsync(int id)
        {
            if (id <= 0)
                return new MyTaskDto();

            var model = await _context.Tasks
            .Include(c => c.Category)
            .Include(p => p.Priory)
            .SingleOrDefaultAsync(t => t.Id == id) ?? new MyTask();

            if (model.Id == default)
                return new MyTaskDto();

            return new MyTaskDto
            {
                Id = model.Id,
                TitleTask = model.TitleTask,
                Category = model.Category.Name,
                Prior = (PriorityFrom)Enum.Parse(typeof(PriorityFrom), model.Priory.Name),
                Expiration = model.Expiration
            };
        }

        public async Task AddTaskAsync(MyTaskDto taskDto)
        {
            taskDto.Id = _context.Tasks.Count();

            var task = new MyTask
            {
                TitleTask = taskDto.TitleTask,
                Expiration = taskDto.Expiration,
                Category = new Category { Name = taskDto.Category ?? string.Empty, Description = taskDto.CategoryDescription ?? string.Empty },
                Priory = new Priority { Name = taskDto.Prior.ToString() }
            };

            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task TaskUpdate(int oldTaskId, SmallTaskDTO std)
        {
            var oldTask = _context.Tasks.Find(oldTaskId);

            if (oldTask.Id == 0)
                throw new Exception("Don't Found");
            else
            {
                oldTask.TitleTask = std.Title;
                oldTask.Expiration = std.Expiration;
                
                await _context.SaveChangesAsync();
            }

        }

        public async Task Delete(MyTaskDto task)
        {
            var taskId = await _context.Tasks
                .Include(c => c.Category)
                .Include(p => p.Priory)
                .SingleOrDefaultAsync(t => t.Id == task.Id);

            if (taskId is not null)
            {   
                _context.Tasks.Remove(taskId);
                _context.SaveChanges();
            }
        }
    }
}
