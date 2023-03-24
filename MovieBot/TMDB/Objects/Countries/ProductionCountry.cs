using Newtonsoft.Json;

namespace MovieBot.TMDB.Objects.Countries
{
    public class ProductionCountry
    {
        [JsonProperty("iso_3166_1")]
        public string iso_3166_1 { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }
    }
}
