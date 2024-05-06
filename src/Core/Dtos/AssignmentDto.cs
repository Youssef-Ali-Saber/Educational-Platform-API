using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class AssignmentDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public string Type { get; set; }
        public int DeadLine { get; set; }
        public int Grade { get; set; }
        public int CourseId { get; set; }
    }
}
