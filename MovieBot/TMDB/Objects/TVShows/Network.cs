using Newtonsoft.Json;

namespace MovieBot.TMDB.Objects.TVShows
{
    public class Network
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("logo_path")]
        public string logoPath { get; set; }

        [JsonProperty("origin_country")]
        public string originCountry { get; set; }
    }
}
