using Helpers;
using Models.EfClasses;
using System.Text.Json.Serialization;

namespace MyTaskManager.Models.DTO.TaskDTO
{
    public class CreateTaskResponse
    {
        public int Id { get; set; }
        public string TitleTask { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PriorityFrom Prior { get; set; }
        public DateTime Expiration { get; set; } = DateTime.Now;

        [JsonIgnore]
        public string PriorityDescription
        {
            get { return new TaskHelper().GetPriorityDescription(Prior); }
        }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
