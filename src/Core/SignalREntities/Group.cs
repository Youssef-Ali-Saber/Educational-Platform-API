
using System.ComponentModel.DataAnnotations;

namespace Core.SignalREntities
{
    public class Group
    {
        public string Name { get; set; }
        [Key]
        public int CourseId  { get; set; }
    }
}
