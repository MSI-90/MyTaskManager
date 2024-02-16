using MyTaskManager.Data;

namespace MyTaskManager.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<MyTask>> GetAllTasksAsync();
        Task<MyTask> GetTaskAsync(int id);
        Task<MyTask> AddTaskAsync(MyTask task);
    }
}
