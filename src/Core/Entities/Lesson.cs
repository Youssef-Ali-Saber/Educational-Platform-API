﻿
namespace Core.Entities
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int ChapterId { get; set; }
        public Chapter Chapter { get; set; }
        public IEnumerable<Resource> Resources { get; set; }
    }
}
