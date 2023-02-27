using System.ComponentModel.DataAnnotations;

namespace ServerAnalytics.Models
{
    public class WorkLodaProcessor
    {
        [Key]
        public int Id { get; set; }
        public double WorkLoda { get; set; }
        public DateTime DateCheck { get; set; }
    }
}
