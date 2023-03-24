using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MovieBot.TMDB.Objects.Search
{
    public class SearchMovie : SearchBase, IListable
    {
        private readonly MediaType mediaType = MediaType.Movie;

        private readonly string emoji = "🎥";

        [JsonProperty("adult")]
        public bool adult { get; set; }

        [JsonProperty("overview")]
        public string overview { get; set; }

        [JsonProperty("release_date")]
        public DateTime? releaseDate { get; set; }

        [JsonProperty("genre_ids")]
        public List<int> genreIds { get; set; }

        [JsonProperty("original_title")]
        public string originalTitle { get; set; }

        [JsonProperty("originalLanguage")]
        public string originalLanguage { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("popularity")]
        public float popularity { get; set; }

        [JsonProperty("vote_count")]
        public int voteCount { get; set; }

        [JsonProperty("video")]
        public bool video { get; set; }

        [JsonProperty("vote_average")]
        public float voteAverage { get; set; }

        public int getID() => id;

        public string getName() => title;

        public DateTime? getReleaseDate() => releaseDate;

        public float getRating() => voteAverage;

        public MediaType getMediaType() => mediaType;

        public string getEmoji() => emoji;
    }
}
