
namespace Core.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public string? PaymentIntentId { get; set; }
        public string PaymentStatus { get; set; }
        public string? SessionId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string StudentId { get; set; }
        public AppUser Student { get; set; }
    }
}
