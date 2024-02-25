using MyTaskManager.Data;
using MyTaskManager.DTO;

namespace MyTaskManager.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<MyTask>> GetAllTasksAsync();
        Task<MyTask> GetTaskAsync(int id);
        Task<MyTask> AddTaskAsync(MyTaskDto task);
        Task TaskUpdate(int oldTaskId, SmallTaskDTO taskDto);
        Task Delete(MyTask task);
    }
}
