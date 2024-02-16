using Microsoft.EntityFrameworkCore;
using MyTaskManager.Data;
using MyTaskManager.DTO;
using MyTaskManager.Repositories.Interfaces;

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
                .Include(u => u.User)
                .ToListAsync();
        }
            

        public async Task<MyTask> GetTaskAsync(int id)
        {
            if (id <= 0)
                return new MyTask();

                return await _context.Tasks
                .Include(c => c.Category)
                .Include(p => p.Priory)
                .Include(u => u.User)
                .SingleOrDefaultAsync(t => t.Id == id) ?? new MyTask();
        }

        public async Task<MyTask> AddTaskAsync(MyTask newTask)
        {
            await _context.Tasks.AddAsync(newTask);
            await _context.SaveChangesAsync();

            return newTask;
        }
        public void Delete(int id)
        {
            var taskId = _context.Tasks.Find(id);
            if (taskId is not null)
            {
                _context.Tasks.Remove(taskId);
                _context.SaveChanges();
            }
        }

        public async Task<MyTask> AddTaskAsync(MyTaskDto taskDto)
        {
            taskDto.Id = _context.Tasks.Count();

            var task = new MyTask
            {
                TitleTask = taskDto.TitleTask,
                Expiration = taskDto.Expiration,
                Category = new Category { Name = taskDto.Category, Description = taskDto.CategoryDescription},
                Priory = new Priority { Name = taskDto.Priory, Descrption = taskDto.PriorityDescription },
                User = new User 
                {
                    FirstName = taskDto.UserFirstName, LastName = taskDto.UserLastName
                }
            };

            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            return task;
        }
    }
}
