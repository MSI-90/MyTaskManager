using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Models.EfClasses;
using MyTaskManager.Controllers;
using MyTaskManager.EfCode;
using MyTaskManager.Models;
using MyTaskManager.Models.DTO.TaskDTO;
using MyTaskManager.Repositories.Interfaces;
using MyTaskManager.Services.Interfaces;

namespace MyTaskManager.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskContext _context;
        private readonly IGetUserIdentity _userIdentity;
        private readonly IStringLocalizer<TasksController> _stringLocalizer;
        public TaskRepository(TaskContext context, IGetUserIdentity userIdentity, IStringLocalizer<TasksController> stringLocalizer)
        {
            _context = context;
            _userIdentity = userIdentity;
            _stringLocalizer = stringLocalizer;
        }
        public async Task<IEnumerable<MyTaskDto>> GetAllTasksAsync()
        {
            IEnumerable<string> decodeClaims = _userIdentity.GetClaims();
            if (!decodeClaims.Any())
                return Enumerable.Empty<MyTaskDto>();

            var userClaim = decodeClaims.ElementAtOrDefault(3);
            var modelFromEntity = await _context.Tasks.Where(t => t.User.Id == Convert.ToInt32(userClaim))
                .Include(c => c.Category)
                .ToListAsync() ?? new List<MyTask>();

            if (!modelFromEntity.Any())
                return Enumerable.Empty<MyTaskDto>();

            var taskDto = modelFromEntity.Select(task => new MyTaskDto
            {
                Id = task.Id,
                TitleTask = task.TitleTask,
                Category = task.Category.Name,
                CategoryDescription = task.Category.Description,
                Prioriry = task.Priority,
                Expiration = task.Expiration
            });

            return taskDto.OrderBy(task => task.Id);
        }

        public async Task<MyTaskDto> GetTaskAsync(int id)
        {
            if (id <= 0)
                return new MyTaskDto();

            IEnumerable<string> decodeClaims = _userIdentity.GetClaims();
            var userClaim = decodeClaims.ElementAtOrDefault(3);

            var model = await _context.Tasks.Where(t => t.User.Id == Convert.ToInt32(userClaim))
            .Include(c => c.Category)
            .FirstOrDefaultAsync(t => t.Id == id) ?? new MyTask();

            if (model.Id == default)
                return new MyTaskDto();

            return new MyTaskDto
            {
                Id = model.Id,
                TitleTask = model.TitleTask,
                Category = model.Category.Name,
                CategoryDescription = model.Category.Description,
                Prioriry = model.Priority,
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
                {
                    string serverError = _stringLocalizer["TaskIsSet"].Value;
                    string error = string.Format(serverError, uniqueTitleTask.TitleTask);
                    throw new Exception(error);
                }

                var dateFromRequest = new DateTime();
                dateFromRequest = taskDto.Expiration;
                if (dateFromRequest < DateTime.Now)
                {
                    string serverError = _stringLocalizer["DateTimeIsLow"].Value;
                    string error = string.Format(serverError, dateFromRequest, DateTime.Now);
                    throw new Exception(error);
                }

                var user = await _context.Users.Include(u => u.Categories).FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                    return new CreateTaskResponse();

                var categoryExist = user.Categories.FirstOrDefault(c => c.Name.ToLower() == taskDto.CategoryName.ToLower()) ??
                    new Category { Name = taskDto.CategoryName, Description = taskDto.CategoryDescription };

                var task = new MyTask
                {
                    TitleTask = taskDto.TitleTask,
                    Expiration = taskDto.Expiration,
                    Category = (Category)categoryExist,
                    Priority = taskDto.Prior,
                    User = user
                };

                user.Categories.Add(categoryExist);

                await _context.Tasks.AddAsync(task);
                await _context.SaveChangesAsync();

                return new CreateTaskResponse
                {
                    Id = task.Id,
                    TitleTask = task.TitleTask,
                    Expiration = task.Expiration,
                    CategoryName = task.Category.Name,
                    CategoryDescription = task.Category.Description,
                    Prior = taskDto.Prior,
                    UserId = task.User.Id,
                    UserName = task.User.UserName
                };
            }
            return new CreateTaskResponse();
        }

        public async Task TaskUpdate(int oldTaskId, UpdateTaskDTO taskUpdate)
        {
            var oldTask = _context.Tasks.Find(oldTaskId);

            if (oldTask != null)
            {
                var categoryExist = await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == taskUpdate.CategoryName.ToLower());
                var category = categoryExist ?? new Category { Name = taskUpdate.CategoryName, Description = taskUpdate.CategoryDescription };

                oldTask.TitleTask = taskUpdate.Title;
                oldTask.Category = category;
                oldTask.Priority = taskUpdate.Priority;
                oldTask.Expiration = taskUpdate.Expiration;

                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(MyTaskDto task)
        {
            var taskId = await _context.Tasks
                .Include(c => c.Category)
                .SingleOrDefaultAsync(t => t.Id == task.Id);

            if (taskId is not null)
            {
                _context.Tasks.Remove(taskId);
                _context.SaveChanges();
            }
        }
    }
}
