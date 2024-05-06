using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Resource
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileLink { get; set; }
        public string FileName { get; set; }
        [NotMapped]
        public IFormFile file { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}
