using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string Language { get; set; }
        public string Level { get; set; }
        public int Duration { get; set; }
        public Types Type { get; set; }
        public string InstructorId { get; set; }
        public DateTime DateOfPublish { get; set; }
        public virtual IEnumerable<AppUser>? AppUsers { get; set; }
        public virtual IEnumerable<Assignment>? Assignments { get; set; }
        public virtual IEnumerable<Chapter>? Chapters { get; set; }

    }
    public enum Types
    {
        Free,
        Paid
    }
}
