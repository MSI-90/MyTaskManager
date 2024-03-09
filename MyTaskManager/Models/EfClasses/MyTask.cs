using System.ComponentModel.DataAnnotations;

namespace Models.EfClasses
{
    public class MyTask
    {
        public int Id { get; set; }
        
        [MaxLength(50)]
        public string TitleTask { get; set; } = string.Empty;
        public Category? Category { get; set; }
        public Priority? Priory { get; set; }
        public DateTime Expiration { get; set; }
        public User? User { get; set; }
    }
}
