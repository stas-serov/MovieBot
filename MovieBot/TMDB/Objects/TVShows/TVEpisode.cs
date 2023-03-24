using Newtonsoft.Json;
using System;

namespace MovieBot.TMDB.Objects.TVShows
{
    public class TVEpisode
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("air_date")]
        public DateTime airDate { get; set; }

        [JsonProperty("episode_number")]
        public int episodeNumber { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("overview")]
        public string overview { get; set; }

        [JsonProperty("production_code")]
        public string productionCode { get; set; }

        [JsonProperty("season_number")]
        public int seasonNumber { get; set; }

        [JsonProperty("still_path")]
        public string stillPath { get; set; }

        [JsonProperty("vote_average")]
        public float voteAverage { get; set; }

        [JsonProperty("vote_count")]
        public int voteCount { get; set; }
    }
}
