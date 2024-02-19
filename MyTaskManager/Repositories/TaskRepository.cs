using Microsoft.EntityFrameworkCore;
using MyTaskManager.Data;
using MyTaskManager.DTO;
using MyTaskManager.Repositories.Interfaces;
using System.Threading.Tasks;

namespace MyTaskManager.Repositories
{
    internal class TaskRepository : ITaskRepository
    {
        private readonly TaskContext _context;
        public TaskRepository(TaskContext context) => _context = context;

        public async Task<IEnumerable<MyTask>> GetAllTasksAsync()
        {
            return await _context.Tasks
                .Include(c => c.Category)
                .Include(p => p.Priory)
                .ToListAsync();
        }

        public async Task<MyTask> GetTaskAsync(int id)
        {
            if (id <= 0)
                return new MyTask();

                return await _context.Tasks
                .Include(c => c.Category)
                .Include(p => p.Priory)
                .SingleOrDefaultAsync(t => t.Id == id) ?? new MyTask();
        }

        public async Task<MyTask> AddTaskAsync(MyTaskDto taskDto)
        {
            taskDto.Id = _context.Tasks.Count();

            var task = new MyTask
            {
                TitleTask = taskDto.TitleTask,
                Expiration = taskDto.Expiration,
                Category = new Category { Name = taskDto.Category, Description = taskDto.CategoryDescription },
                Priory = new Priority { Name = taskDto.Priority.ToString() ?? String.Empty }
            };

            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            return task;
        }

        public async Task Delete(MyTask task)
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
