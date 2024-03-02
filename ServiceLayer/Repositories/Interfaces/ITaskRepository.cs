﻿using MyTaskManager.Data;
using MyTaskManager.DTO;

namespace MyTaskManager.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<MyTask>> GetAllTasksAsync();
        Task<MyTaskDto> GetTaskAsync(int id);
        Task<MyTask> AddTaskAsync(MyTaskDto task);
        Task TaskUpdate(int oldTaskId, SmallTaskDTO taskDto);
        Task Delete(MyTaskDto task);
    }
}