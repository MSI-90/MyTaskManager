using MyTaskManager.Models.DTO.TaskDTO;

namespace MyTaskManager.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<MyTaskDto>> GetAllTasksAsync();
        Task<MyTaskDto> GetTaskAsync(int id);
        Task AddTaskAsync(CreateTaskRequest task);
        Task TaskUpdate(int oldTaskId, SmallTaskDTO taskDto);
        Task Delete(MyTaskDto task);
    }
}
