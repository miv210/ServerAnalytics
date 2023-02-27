using System.ComponentModel.DataAnnotations;

namespace ServerAnalytics.Models
{
    public class MemoryMetric
    {
        [Key]
        public int id { get; set; }
        [Required]
        public double Total { get; set; }
        [Required]
        public double Used { get; set; }
        [Required]
        public double? Free { get; set; }

        public DateTime DateCheck { get; set; }
    }
}
