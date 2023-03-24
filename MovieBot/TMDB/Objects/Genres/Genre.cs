using Newtonsoft.Json;

namespace MovieBot.TMDB.Objects.Genres
{
    public class Genre
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }
    }
}