using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Submission
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public string? Link { get; set; }
        [Required]
        public DateTime DateOfSubmission { get; set; } = DateTime.Now;
        public int AssignmentId { get; set; }
        public string StudentId { get; set; }
        public int Grade { get; set; }
        public Assignment Assignment { get; set; }
        public AppUser Student { get; set; }
    }
}
