using Newtonsoft.Json;
using System;

namespace MovieBot.TMDB.Objects.TVShows
{
    public class Season
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("air_date")]
        public DateTime? airDate { get; set; }

        [JsonProperty("episode_count")]
        public int epicodeCount { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("overview")]
        public string overview { get; set; }

        [JsonProperty("poster_path")]
        public string posterPath { get; set; }

        [JsonProperty("season_number")]
        public int seasonNumber { get; set; }
    }
}
