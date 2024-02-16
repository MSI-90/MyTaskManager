using MyTaskManager.Data;

namespace MyTaskManager.Repositories.Interfaces
{
    internal interface ITaskRepository
    {
        Task<MyTask> AddTask(MyTask task);
    }
}
