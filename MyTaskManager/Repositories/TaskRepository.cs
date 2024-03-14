using Microsoft.EntityFrameworkCore;
using Models.EfClasses;
using MyTaskManager.EfCode;
using MyTaskManager.Models;
using MyTaskManager.Models.DTO.TaskDTO;
using MyTaskManager.Repositories.Interfaces;
using MyTaskManager.Services;
using MyTaskManager.Services.Interfaces;
using System.Drawing.Text;
using System.Security.Claims;

namespace MyTaskManager.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskContext _context;
        private readonly IGetUserIdentity _userIdentity;
        public TaskRepository(TaskContext context, IGetUserIdentity userIdentity)
        {
            _context = context;
            _userIdentity = userIdentity;
        }

        public async Task<IEnumerable<MyTaskDto>> GetAllTasksAsync()
        {
            var modelFromEntity = await _context.Tasks/*.Where(t => t.UserId == Convert.ToInt32(decodeClaims[3]))*/
                .Include(c => c.Category)
                .Include(p => p.Priory)
                .ToListAsync() ?? throw new Exception();

            if (modelFromEntity.Count == 0)
                return Enumerable.Empty<MyTaskDto>();

            var taskDto = modelFromEntity.Select(task => new MyTaskDto
            {
                Id = task.Id,
                TitleTask = task.TitleTask,
                Category = task.Category.Name,
                CategoryDescription = task.Category.Description,
                Prior = (PriorityFrom)Enum.Parse(typeof(PriorityFrom), task.Priory.Name),
                Expiration = task.Expiration
            });

            return taskDto.OrderBy(task => task.Id);
        }

        public async Task<MyTaskDto> GetTaskAsync(int id)
        {
            if (id <= 0)
                return new MyTaskDto();

            var model = await _context.Tasks
            .Include(c => c.Category)
            .Include(p => p.Priory)
            .FirstOrDefaultAsync(t => t.Id == id) ?? new MyTask(); //throw new Exception();

            if (model.Id == default)
                return new MyTaskDto();

            return new MyTaskDto
            {
                Id = model.Id,
                TitleTask = model.TitleTask,
                Category = model.Category.Name,
                Prior = (PriorityFrom)Enum.Parse(typeof(PriorityFrom), model.Priory.Name),
                Expiration = model.Expiration
            };
        }

        public async Task<CreateTaskResponse> AddTaskAsync(CreateTaskRequest taskDto)
        {
            IEnumerable<string> decodeClaims = _userIdentity.GetClaims();
            var userClaim = decodeClaims.ElementAtOrDefault(3);
            if (userClaim != null && int.TryParse(userClaim, out int userId))
            {
                var uniqueTitleTask = await _context.Tasks
                    .Where(t => t.User.Id == userId)
                    .FirstOrDefaultAsync(t => t.TitleTask == taskDto.TitleTask);

                if (uniqueTitleTask != null)
                    throw new Exception($"Задача, именуемая как - {uniqueTitleTask.TitleTask} уже существует.");

                var user = _context.Users.FirstOrDefault(u => u.Id == userId);

                var categoryExist = await _context.Categories.FirstOrDefaultAsync(c => c.Name == taskDto.CategoryName);
                var category = categoryExist ?? new Category { Name = taskDto.CategoryName ?? string.Empty, Description = taskDto.CategoryDescription ?? string.Empty };

                var priorityExist = await _context.Priority.FirstOrDefaultAsync(p => p.Name == taskDto.Prior.ToString());
                var priority = priorityExist ?? new Priority { Name = taskDto.Prior.ToString() };

                var task = new MyTask
                {
                    TitleTask = taskDto.TitleTask,
                    Expiration = taskDto.Expiration,
                    Category = (Category)category,
                    Priory = (Priority)priority,
                    User = user
                };

                await _context.Tasks.AddAsync(task);
                await _context.SaveChangesAsync();

                return new CreateTaskResponse
                {
                    Id = task.Id,
                    TitleTask = task.TitleTask,
                    Expiration = task.Expiration,
                    CategoryName = task.Category.Name,
                    Prior = Enum.Parse<PriorityFrom>(task.Priory.Name),
                    UserId = task.User.Id,
                    UserName = task.User.UserName
                };
            }
            return new CreateTaskResponse();
        }

        public async Task TaskUpdate(int oldTaskId, SmallTaskDTO std)
        {
            var oldTask = _context.Tasks.Find(oldTaskId);

            if (oldTask.Id == 0)
                throw new Exception("Don't Found");
            else
            {
                oldTask.TitleTask = std.Title;
                oldTask.Expiration = std.Expiration;

                await _context.SaveChangesAsync();
            }

        }

        public async Task Delete(MyTaskDto task)
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
