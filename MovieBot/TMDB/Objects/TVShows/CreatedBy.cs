using Newtonsoft.Json;

namespace MovieBot.TMDB.Objects.TVShows
{
    public class CreatedBy
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("credit_id")]
        public string creditId { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("gender")]
        public int gender { get; set; }

        [JsonProperty("profile_path")]
        public string profilePath { get; set; }
    }
}
