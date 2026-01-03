namespace PersonalWebsite.Models.DataObjects
{
    public class Owner
    {
        public required string login { get; set; }
        public required int id { get; set; }
        public required string node_id { get; set; }
        public required string avatar_url { get; set; }
        public required string gravatar_id { get; set; }
        public required string url { get; set; }
        public required string html_url { get; set; }
        public required string followers_url { get; set; }
        public  required string following_url { get; set; }
        public required string gists_url { get; set; }
        public required string starred_url { get; set; }
        public required string subscriptions_url { get; set; }
        public required string organizations_url { get; set; }
        public required string repos_url { get; set; }
        public required string events_url { get; set; }
        public required string received_events_url { get; set; }
        public required string type { get; set; }
        public required string user_view_type { get; set; }
        public bool site_admin { get; set; }
    }
}
