using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerAnalytics.Models
{
    public class WorkLodaProcessor
    {
        [Key]
        public int Id { get; set; }
        public double WorkLoda { get; set; }
        public int IdServer { get; set; }

        [ForeignKey("IdServer")]
        public Server? Server { get; set; }
        public DateTime DateCheck { get; set; }
    }
}
