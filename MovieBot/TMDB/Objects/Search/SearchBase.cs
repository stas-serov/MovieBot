using Newtonsoft.Json;

namespace MovieBot.TMDB.Objects.Search
{
    public abstract class SearchBase
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("poster_path")]
        public string posterPath { get; set; }

        [JsonProperty("backdrop_path")]
        public string backdropPath { get; set; }
    }
}
