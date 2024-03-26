using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.DotNet.Scaffolding.Shared.T4Templating;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Models.EfClasses;
using MyTaskManager.Controllers;
using MyTaskManager.EfCode;
using MyTaskManager.Models;
using MyTaskManager.Models.DTO.TaskDTO;
using MyTaskManager.Repositories.Interfaces;
using MyTaskManager.Services.Interfaces;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;

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
                //ищем пользователя и включаем в результат список категорий задач, если они имеются у пользователя
                var user = await _context.Users.Include(u => u.Categories).FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    string error = _stringLocalizer["DontUser"].Value;
                    throw new Exception(error);
                }

                //имеется ли уже задача у пользователя в таблице БД?
                var uniqueTitleTask = await _context.Tasks
                    .Where(t => t.User.Id == userId)
                    .FirstOrDefaultAsync(t => t.TitleTask == taskDto.TitleTask);

                //если есть
                if (uniqueTitleTask != null)
                {
                    string serverError = _stringLocalizer["TaskIsSet"].Value;
                    string error = string.Format(serverError, uniqueTitleTask.TitleTask);
                    throw new Exception(error);
                }

                //Проверка на значение указаное как приоритет задачи
                if (!Enum.IsDefined(typeof(PriorityFrom), taskDto.Prior))
                {
                    var serverError = _stringLocalizer["ValueDontPrefer"];
                    var error = string.Format(serverError, taskDto.Prior);
                    throw new Exception(error);
                }

                //проверка на дату-время планируемых у задачи как дата и время к закрытию задачи
                var dateFromRequest = taskDto.Expiration;
                if (dateFromRequest < DateTime.Now)
                {
                    string serverError = _stringLocalizer["DateTimeIsLow"].Value;
                    string error = string.Format(serverError, dateFromRequest, DateTime.Now);
                    throw new Exception(error);
                }

                var categoryExist = user.Categories.FirstOrDefault(c => c.Name.ToLower() == taskDto.CategoryName.ToLower()) ??
                    new Category { Name = taskDto.CategoryName, Description = taskDto.CategoryDescription };

                var task = new MyTask
                {
                    TitleTask = taskDto.TitleTask,
                    Expiration = taskDto.Expiration,
                    Category = (Category)categoryExist,
                    Priority = Enum.Parse<PriorityFrom>(taskDto.Prior),
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
                    Prior = Enum.Parse<PriorityFrom>(taskDto.Prior),
                    UserId = task.User.Id,
                    UserName = task.User.UserName
                };
            }
            return new CreateTaskResponse();
        }

        public async Task TaskUpdate(int id, UpdateTaskDTO taskUpdate)
        {
            var oldTask = await _context.Tasks.SingleOrDefaultAsync(c => c.Id == id);

            if (oldTask.Id > 0 && !string.IsNullOrEmpty(oldTask.TitleTask))
            {
                var dt = DateTime.Now;
                var taskDate = taskUpdate.Expiration < oldTask.Expiration ? DateTime.Now.AddHours(1) : taskUpdate.Expiration;

                oldTask.TitleTask = string.IsNullOrEmpty(taskUpdate.Title) ? oldTask.TitleTask : taskUpdate.Title;
                oldTask.Category.Name = string.IsNullOrEmpty(taskUpdate.CategoryName) ? oldTask.Category.Name : taskUpdate.CategoryName;
                oldTask.Category.Description = string.IsNullOrEmpty(taskUpdate.CategoryDescription) ? oldTask.Category.Description : taskUpdate.CategoryDescription;
                oldTask.Priority = Enum.IsDefined(typeof(PriorityFrom), taskUpdate.Priority) ? oldTask.Priority : taskUpdate.Priority;
                oldTask.Expiration = DateTime.SpecifyKind(taskDate, DateTimeKind.Utc);

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