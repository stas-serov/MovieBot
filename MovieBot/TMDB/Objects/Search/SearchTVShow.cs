using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MovieBot.TMDB.Objects.Search
{
    public class SearchTVShow : SearchBase, IListable
    {
        private readonly MediaType mediaType = MediaType.TVShow;

        private readonly string emoji = "📺";

        [JsonProperty("popularity")]
        public float popularity { get; set; }

        [JsonProperty("vote_average")]
        public float voteAverage { get; set; }

        [JsonProperty("overview")]
        public string overview { get; set; }

        [JsonProperty("first_air_date")]
        public DateTime? firstAirDate { get; set; }

        [JsonProperty("origin_country")]
        public List<string> originCountry { get; set; }

        [JsonProperty("genre_ids")]
        public List<int> genreIds { get; set; }

        [JsonProperty("originalLanguage")]
        public string originalLanguage { get; set; }

        [JsonProperty("vote_count")]
        public int voteCount { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("original_name")]
        public string originalName { get; set; }

        public int getID() => id;

        public string getName() => name;

        public float getRating() => voteAverage;

        public DateTime? getReleaseDate() => firstAirDate;

        public MediaType getMediaType() => mediaType;

        public string getEmoji() => emoji;
    }
}
