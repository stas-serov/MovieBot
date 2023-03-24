using Newtonsoft.Json;

namespace MovieBot.TMDB.Objects.Movies
{
    public class SpokenLanguage
    {
        [JsonProperty("iso_639_1")]
        public string iso_639_1 { get; set; }

        [JsonProperty("name")]
        public string mame { get; set; }
    }
}
