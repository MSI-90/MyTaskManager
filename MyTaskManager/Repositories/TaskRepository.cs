using Microsoft.EntityFrameworkCore;
using MyTaskManager.Data;
using MyTaskManager.Repositories.Interfaces;

namespace MyTaskManager.Repositories
{
    internal class TaskRepository : ITaskRepository
    {
        private readonly TaskContext _context;
        public TaskRepository(TaskContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MyTask>> GetAllTasksAsync() => await _context.Tasks.ToListAsync();

        public async Task<MyTask> GetTaskAsync(int id)
        {
            if (id <= 0)
                return new MyTask();

                return await _context.Tasks.SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<MyTask> AddTaskAsync(MyTask newTask)
        {
            await _context.Tasks.AddAsync(newTask);
            await _context.SaveChangesAsync();

            return newTask;
        }
    }
}
