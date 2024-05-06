using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class SubmissionDto
    {
        public string Content { get; set; }
        public string Link { get; set; }
        public string Comment { get; set; }
        public int AssignmentId { get; set; }
    }
}
