using MovieBot.TMDB.Objects.Genres;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using MovieBot.TMDB.Objects.Companies;
using MovieBot.TMDB.Objects.Countries;
using MovieBot.TMDB.Objects.Languages;

namespace MovieBot.TMDB.Objects.TVShows
{
    public class TVShowDetails
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("backdrop_path")]
        public string backdropPath { get; set; }

        [JsonProperty("created_by")]
        public List<CreatedBy> createdBy { get; set; }

        [JsonProperty("episode_run_time")]
        public List<int> episodeRuntime { get; set; }

        [JsonProperty("first_air_date")]
        public DateTime? firstAirDate { get; set; }

        public List<Genre> genres { get; set; }

        [JsonProperty("homepage")]
        public string homepage { get; set; }

        [JsonProperty("in_production")]
        public bool inProduction { get; set; }

        [JsonProperty("languages")]
        public List<string> languages { get; set; }

        [JsonProperty("last_air_date")]
        public DateTime? lastAirDate { get; set; }

        [JsonProperty("last_episode_to_air")]
        public TVEpisode lastEpisodeToAir { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("next_episode_to_air")]
        public TVEpisode nextEpisodeToAir { get; set; }

        [JsonProperty("networks")]
        public List<Network> networks { get; set; }

        [JsonProperty("number_of_episodes")]
        public int numberOfEpisodes { get; set; }

        [JsonProperty("number_of_seasons")]
        public int numberOfSeasons { get; set; }

        [JsonProperty("origin_country")]
        public List<string> originCountry { get; set; }

        [JsonProperty("originalLanguage")]
        public string originalLanguage { get; set; }

        [JsonProperty("original_name")]
        public string originalName { get; set; }

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

        [JsonProperty("seasons")]
        public List<Season> seasons { get; set; }

        [JsonProperty("spoken_languages")]
        public List<Language> spokenLanguages { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("tagline")]
        public string tagline { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("vote_average")]
        public float voteAverage { get; set; }

        [JsonProperty("vote_count")]
        public int voteCount { get; set; }
    }
}
