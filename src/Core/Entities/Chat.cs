

namespace Core.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public AppUser User { get; set; }
        public Course Course { get; set; }
    }
}
