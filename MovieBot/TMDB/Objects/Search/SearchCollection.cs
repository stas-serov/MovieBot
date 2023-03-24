using Newtonsoft.Json;

namespace MovieBot.TMDB.Objects.Search
{
    public class SearchCollection : SearchBase
    {
        [JsonProperty("name")]
        public string name { get; set; }
    }
}
