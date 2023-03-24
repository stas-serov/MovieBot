using Newtonsoft.Json;

namespace MovieBot.TMDB.Objects.Credits
{
    public class Crew
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("adult")]
        public bool adult { get; set; }

        [JsonProperty("gender")]
        public int gender { get; set; }

        [JsonProperty("known_for_department")]
        public string knownForDepartment { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("original_name")]
        public string originalName { get; set; }

        [JsonProperty("popularity")]
        public long popularity { get; set; }

        [JsonProperty("profile_path")]
        public string profilePath { get; set; }

        [JsonProperty("credit_id")]
        public string creditId { get; set; }

        [JsonProperty("department")]
        public string department { get; set; }

        [JsonProperty("job")]
        public string job { get; set; }
    }
}
