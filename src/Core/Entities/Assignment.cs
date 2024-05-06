using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Assignment
    {
        public int Id { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Type { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime DateOfLastModification { get; set; }
        public int Deadline { get; set; }
        public int Grade { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }

    }
}
