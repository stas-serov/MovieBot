using MovieBot.TMDB.Objects.Companies;
using MovieBot.TMDB.Objects.Countries;
using MovieBot.TMDB.Objects.Genres;
using MovieBot.TMDB.Objects.Search;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MovieBot.TMDB.Objects.Movies
{
    public class MovieDetails
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("adult")]
        public bool adult { get; set; }

        [JsonProperty("backdrop_path")]
        public string backdropPath { get; set; }

        [JsonProperty("belongs_to_collections")]
        public SearchCollection belongsToCollections { get; set; }

        [JsonProperty("budget")]
        public Int128 budget { get; set; }

        [JsonProperty("genres")]
        public List<Genre> genres { get; set; }

        [JsonProperty("homepage")]
        public string homepage { get; set; }

        [JsonProperty("imdb_id")]
        public string imdbId { get; set; }

        [JsonProperty("original_language")]
        public string originalLanguage { get; set; }

        [JsonProperty("original_title")]
        public string originalTitle { get; set; }

        [JsonProperty("overview")]
        public string overview { get; set; }

        [JsonProperty("popularity")]
        public float popularity { get; set; }

        [JsonProperty("poster_path")]
        public string posterPath { get; set; }

        [JsonProperty("production_companies")]
        public List<ProductionCompany> productionCompanies { get; set; }

        [JsonProperty("production_countries")]
        public List<ProductionCountry> productionCountries { get; set; }

        [JsonProperty("release_date")]
        public DateTime? releaseDate { get; set; }

        [JsonProperty("revenue")]
        public Int128 revenue { get; set; }

        [JsonProperty("runtime")]
        public int runtime { get; set; }

        [JsonProperty("spoken_languages")]
        public List<SpokenLanguage> spokenLanguages { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("tagline")]
        public string tagline { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("video")]
        public bool video { get; set; }

        [JsonProperty("vote_average")]
        public float voteAverage { get; set; }

        [JsonProperty("vote_count")]
        public int voteCount { get; set; }
    }
}
