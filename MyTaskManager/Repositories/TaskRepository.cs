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

        public async Task<MyTask> AddTask(MyTask newTask)
        {
            await _context.Tasks.AddAsync(newTask);
            await _context.SaveChangesAsync();

            return newTask;
        }
    }
}
