using MyTaskManager.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models.EfClasses
{
    public class MyTask
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string TitleTask { get; set; } = string.Empty;
        public Category? Category { get; set; }
        public PriorityFrom Priority { get; set; }
        public DateTime Expiration { get; set; }
        public User User { get; set; }
    }
}
