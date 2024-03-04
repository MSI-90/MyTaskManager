using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyTaskManager.Data
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; } = String.Empty;

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; } = String.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = String.Empty;

        [Required]
        [MaxLength(20)]
        public string Password { get; set; } = String.Empty;
        public ICollection<MyTask>? Tasks { get; set; }
    }
}
