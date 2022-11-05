namespace JwtWebApi.tables
{
    public class Messages
    {
        public int Id { get; set; }
        public string status { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
        public DateTime start_date { get; set; }
        public string data { get; set; } = string.Empty;
        public string desccription { get; set; } = string.Empty;
        public int praiority { get; set; }
        public int mission_id { get; set; }
    }
}
