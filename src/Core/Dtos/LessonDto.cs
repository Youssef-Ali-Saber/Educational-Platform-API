using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class LessonDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string content { get; set; }
        public int ChapterId { get; set; }
    }
}
