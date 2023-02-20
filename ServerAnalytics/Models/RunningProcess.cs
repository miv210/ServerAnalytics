using CsvHelper.Configuration.Attributes;

namespace ServerAnalytics.Models
{
    public class RunningProcess
    {
        public string Id { get; set; }
        [Index(0)]
        public string Name { get; set; }
        [Index(1)]
        public int PID { get; set; }
        [Index(2)]
        public string NameSession { get; set; }
        [Index(3)]
        public int SessionNumber { get; set; }
        [Index(4)]
        public string Memory { get; set; }
        public DateTimeOffset DateChek { get; set; }
    }
}
