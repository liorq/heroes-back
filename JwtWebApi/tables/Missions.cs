namespace JwtWebApi.tables
{
    public class Missions
    {
        public int Id { get; set; }
        public string the_missions { get; set; } = string.Empty;
        public int user_id { get; set; }
        public DateTime start_date { get; set; }
        public DateTime start_work_date { get; set; }
        public DateTime end_date { get; set; }
        public string status { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
        public string missions_message { get; set; } = string.Empty;
        public string note { get; set; } = string.Empty;
        public string photo_path { get; set; } = string.Empty;
        public DateTime should_end { get; set; }
        public string progrem_language { get; set; } = string.Empty;
        public string system { get; set; } = string.Empty;
    }
}
