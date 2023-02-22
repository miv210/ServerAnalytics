namespace ServerAnalytics.Models
{
    public class RunningProcess
    {
        
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public int PID { get; set; }
        
        public string NameSession { get; set; }
        
        public int SessionNumber { get; set; }
        
        public string Memory { get; set; }
        public DateTimeOffset DateChek { get; set; }
    }
}
