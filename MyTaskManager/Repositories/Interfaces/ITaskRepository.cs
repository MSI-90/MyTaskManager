﻿using MyTaskManager.Models.DTO.TaskDTO;

namespace MyTaskManager.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<MyTaskDto>> GetAllTasksAsync();
        Task<MyTaskDto> GetTaskAsync(int id);
        Task<CreateTaskResponse> AddTaskAsync(CreateTaskRequest task);
        Task TaskUpdate(int oldTaskId, UpdateTaskDTO taskUpdate);
        Task Delete(MyTaskDto task);
    }
}
