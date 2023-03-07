namespace ServerAnalyticsApp.Models
{
    public class Server
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }

        public List<RunningProcess> Runnings { get; set; } = new();
    }
}
