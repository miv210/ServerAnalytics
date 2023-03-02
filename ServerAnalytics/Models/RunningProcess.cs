using System.ComponentModel.DataAnnotations;

namespace ServerAnalytics.Models
{
    public class RunningProcess
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public int? PID { get; set; }
        
        public string? NameSession { get; set; }
        
        public int? SessionNumber { get; set; }
        
        public int Memory { get; set; }
        public DateTime DateCheck { get; set; }

        public List<RunningProcess> Children { get; set; } = new();
    }
}
